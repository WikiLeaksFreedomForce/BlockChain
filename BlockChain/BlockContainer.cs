using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BlockChain.Enums;
using BlockChain.Extensions;

namespace BlockChain {

    public class BlockContainer : List<Block>{

        public BlockContainer(){
            TransactionList = new Dictionary<string, Transaction>();
        }

        public Dictionary<string, Transaction> TransactionList { get; }

        public async Task Add(string file){

            var blockHeight = 0;

            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                while (stream.Position < stream.Length) {

                    var blockLength = GetBlockLength(stream);
                    var buffer = new byte[blockLength];

                    await stream.ReadAsync(buffer, 0, (int)blockLength);

                    var block = new Block(buffer){
                        Height = blockHeight++
                    };

                    Add(block);
                }
            }

            // Build a list of transactions

            foreach (var block in this){
                foreach (var transaction in block.Transactions){
                    TransactionList.Add(transaction.ToString(), transaction);
                }
            }
        }

        public void ClearAll(){
            Clear();
            TransactionList?.Clear();
        }

        public void Add(Transaction transaction){
            if (transaction == null)
                return;

            TransactionList.Add(transaction.ToString(), transaction);
        }

        public void JoinInsAndOuts() { 
            // Now link all of the TxIn to the TxOut

            foreach (var block in this){
                foreach (var transaction in block.Transactions){

                    foreach (var txIn in transaction.Ins){

                        var outpoint = txIn.PreviousOutput;
                        var transactionHash = outpoint.TransactionHash.ToString();

                        var previousTransaction = this[transactionHash];

                        if (previousTransaction == null)
                            continue;

                        foreach (var txOut in previousTransaction.Outs){
                            if (txOut.N == outpoint.N){
                                txOut.TxIn = txIn;
                                txIn.TxOut = txOut;
                            }
                        }
                    }
                }
            }
        }

        public Transaction this[string id]{

            get{
                Transaction transaction;

                if (TransactionList == null)
                    return null;

                if (!TransactionList.TryGetValue(id, out transaction)){
                    return null;
                }

                return transaction;
            }
        }

        static uint GetBlockLength(Stream stream) {
            var streamStart = stream.Position;

            // read header and length
            var header = new byte[8];
            stream.Read(header, 0, 8);

            // sometimes the blockfiles have zeros padded extra data that we skip
            // this is mostly an issue with old bitcoin clients

            if (header[0] == 0) {
                var temp = new byte[4096];
                stream.Position = streamStart;
                int read;
                while ((read = stream.Read(temp, 0, 4096)) > 0) {
                    var foundNonzero = false;
                    for (var i = 0; i < read; i++) {
                        if (temp[i] != 0) {
                            foundNonzero = true;
                            streamStart += i;
                            stream.Position = streamStart;
                            if (stream.Read(header, 0, 8) < 8)
                                throw new ApplicationException("The block isn't fully downloaded yet.");
                            break;
                        }
                    }

                    if (foundNonzero)
                        break;

                    streamStart += read;
                }
            }

            var index = 0;
            var magicSignature = header.ToUInt32(ref index);

            if (magicSignature != (uint)Network.Main)
                throw new InvalidOperationException("this code only works on the main blockchain network");

            var blockLength = header.ToUInt32(ref index);

            if (blockLength > 1000000)
                throw new InvalidOperationException();

            return blockLength;
        }

        List<string> _walkedList;

        public string[] WalkAddresses(string root) {

            Transaction transaction;

            if (TransactionList == null)
                return new []{ "No files have been opened"};

            if (!TransactionList.TryGetValue(root, out transaction))
                return new[] { "Transaction Not Found" };

            _walkedList = new List<string>();
            WalkAddresses(transaction);

            return _walkedList.ToArray();
        }

        static string GenesisBlock = "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa";

        void WalkAddresses(Transaction transaction) {

            _walkedList.Add(transaction.ToString());

            foreach (var txOut in transaction.Outs){

                if (txOut.Script?.Address == GenesisBlock)
                    return;

                if (txOut.TxIn == null)
                    continue;

                if (_walkedList.Contains(txOut.TxIn.Transaction.ToString()))
                    continue;

                WalkAddresses(txOut.TxIn.Transaction);
            }
        }

        public FileData GetFile(string txId){
            var transaction = this[txId];

            var bytes = transaction.Outs.GetFileBytes();

            return new FileData(bytes);
        }

        public FileData GetFile(string[] root) {

            var data = new List<byte>();

            foreach (var node in root){

                if (string.IsNullOrEmpty(node))
                    continue;

                var transaction = this[node];

                var bytes = transaction.Outs.GetFileBytes();

                data.AddRange(bytes);
            }

            return new FileData(data.ToArray());
        }

        public FileData GetSatoshiUploadedFile(string root){

            var data = new List<byte>();

            foreach (var address in WalkAddresses(root)) {

                var transaction = this[address];

                var bytes = transaction.GetSatoshiUploadedBytes();

                data.AddRange(bytes);

                // Not real sure about when a Satoshi Upload file ends, but if it's less than 20,000 returned, that's one possible indicator

                if (bytes.Length < 20000)
                    break;
            }

            return new FileData(data.ToArray());
        }

        public FileData DownloadTxInputFile(string tx) {

            var data = new List<byte>();

            var transaction = this[tx];

            if (transaction == null)
                return null;

            var bytes = transaction.DownloadTxInputFile();

            data.AddRange(bytes);

            return new FileData(data.ToArray());
        }
    }
}

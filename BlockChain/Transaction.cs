using System.Collections.Generic;
using System.IO;
using System.Linq;
using BlockChain.Extensions;
using Force.Crc32;

namespace BlockChain
{
    public class Transaction {

        public Hash TransactionHash { get; }

        public uint Version { get; }
        public int Size { get; }
        public LockTime LockTime { get; }

        public TransactionInContainer  Ins { get; }
        public TransactionOutContainer Outs { get; }

        public Transaction(byte[] buffer, ref int index){

            var startIndex = index;

            Version = buffer.ToUInt32(ref index);

            var count = buffer.ToVariableInt(ref index);

            Ins = new TransactionInContainer();

            for (var transaction = 0ul; transaction < count; transaction++){
                Ins.Add(new TransactionIn(buffer, ref index, this));
            }

            count = buffer.ToVariableInt(ref index);

            Outs = new TransactionOutContainer();

            for (var transaction = 0ul; transaction < count; transaction ++)
                Outs.Add(new TransactionOut(buffer, ref index, Outs.Count));

            var isLockTimeIrrelevant = Ins.All(o => o.Sequence == 0xFFFFFFFF);
            LockTime = new LockTime(buffer, ref index, isLockTimeIrrelevant);

            Size = index - startIndex;
            TransactionHash = buffer.ToSha256_2(ref startIndex, Size);
        }

        public BitcoinValue Fees => Ins.Amount - Outs.Amount;
        public BitcoinValue FeePerOut => Fees / Outs.Count;

        public override string ToString(){
            return TransactionHash.ToString();
        }

        public byte[] GetSatoshiUploadedBytes(){

            var data = new List<byte>();

            foreach (var txOut in Outs) {
                data.AddRange(txOut.Script.Inner);
            }

            if (data.Count < 8)
                return null;

            var header = data.GetRange(0, 8).ToArray();

            // Now check headers

            var length = header.ToInt32(0);

            if (length <= 0 || length > data.Count - 8){
                return null;
            }

            var section = data.GetRange(8, length);

            var crc = new Crc32Algorithm();
            var hash = crc.ComputeHash(section.ToArray());

            // Weird comparison is an Endian problem

            if (hash[0] != header[7] || hash[1] != header[6] || hash[2] != header[5] || hash[3] != header[4]) {
                return null;
            }

            return data.GetRange(8, length).ToArray();
        }

        public byte[] DownloadTxInputFile(){

            var data = new List<byte>();

            foreach (var txIn in Ins) {
                data.AddRange(txIn.Script.Inner);
            }

            return data.ToArray();
        }
    }
}
using System.IO;
using System.Threading.Tasks;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {

    public class WalkDownEntireTransaction {

        public WalkDownEntireTransaction(MessageCollection messages){
            _messages = messages;
        }

        readonly MessageCollection _messages;

        BlockContainer _blocks;
        BlockContainer _prevBlocks;

        public async Task Search(string txId, int start, int stop) {

            txId = txId.Trim('\r', '\n');

            _messages.NewLine();
            _messages.Add("Finding Transactions below transaction", MessageType.Heading);
            _messages.Add(txId);

            Transaction transaction = null;

            for (var blockNumber = start; blockNumber <= stop; blockNumber++) {

                var fileName = Path.Combine(Settings.Default.InputPath, $"blk{blockNumber:D5}.dat");

                _messages.Add($"File: {Path.GetFileNameWithoutExtension(fileName)}", MessageType.Heading);

                _blocks = new BlockContainer();
                _blocks.Add(transaction);

                await _blocks.Add(fileName);
                _blocks.JoinInsAndOuts();

                if (txId == "45a285299a48318d53a8c2bba7f47a20a92add7b1e4ca1698acf29b594c8af65"){
                    int j = 0;
                }

                while (true){

                    transaction = _blocks[txId];

                    if (transaction == null && _prevBlocks != null){
                        transaction = _prevBlocks[txId];
                    }

                    if (transaction == null)
                        break;

                    if (transaction.Outs.ValueCount == 0) {
                        _messages.Add($"{transaction} has no valid TxOut", MessageType.Error);
                        return;
                    }

                    if (transaction.Outs.ValueCount > 1){
                        _messages.Add($"{transaction} has more than one valid TxOut", MessageType.Error);
                        return;
                    }

                    var nextTxId = FindTxOut(transaction);

                    if (nextTxId == null){
                        break;
                    }

                    txId = nextTxId;

                    _messages.Add($"{txId}");
                }

                // Since a transaction can actually be in a previous block (shocking), we have to keep the previous blocks open

                _prevBlocks = _blocks;
            }

            _messages.Add("Search Complete", MessageType.Heading);
        }

        string FindTxOut(Transaction transaction){
            foreach (var txOut in transaction.Outs) {
                if (txOut.Value.Btc > 0.00001m){
                    return txOut.TxIn?.Transaction.ToString();
                }
            }

            return null;
        }
    }
}

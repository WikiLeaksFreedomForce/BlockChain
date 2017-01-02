using System.IO;
using System.Threading.Tasks;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {

    public class WalkUpEntireTransaction {

        public WalkUpEntireTransaction(MessageCollection messages){
            _messages = messages;
        }

        readonly MessageCollection _messages;

        public BlockContainer Blocks { get; } = new BlockContainer();

        public async Task Search(string txId, int start, int stop) {

            _messages.NewLine();
            _messages.Add("Finding Transactions above transaction", MessageType.Heading);

            txId = txId.Trim('\r', '\n');

            for (var blockNumber = stop; blockNumber >= start; blockNumber--) {

                var fileName = Path.Combine(Settings.Default.InputPath, $"blk{blockNumber:D5}.dat");

                _messages.Add($"File: {Path.GetFileNameWithoutExtension(fileName)}", MessageType.Heading);

                Blocks.ClearAll();
                await Blocks.Add(fileName);

                while (true){

                    _messages.Add($"{txId}");

                    var transaction = Blocks[txId];

                    if (transaction == null)
                        break;

                    if (transaction.Ins.Count > 1){
                        _messages.Add($"Transaction {transaction} has more than one TxIn... Stopping", MessageType.Error);
                        return;
                    }

                    txId = transaction.Ins[0].PreviousOutput.TransactionHash.ToString();
                }
            }

            _messages.Add("Search Complete", MessageType.Heading);
        }
    }
}

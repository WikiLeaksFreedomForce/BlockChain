using System.IO;
using System.Threading.Tasks;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {

    public class SearchTransactionId{

        public SearchTransactionId(MessageCollection messages){
            _messages = messages;
        }

        private readonly MessageCollection _messages;

        public BlockContainer Blocks { get; } = new BlockContainer();

        public async Task Search(string txId, int start, int stop) {

            txId = txId.Trim('\r', '\n');

            _messages.NewLine();
            _messages.Add($"Searching For Transaction ID {txId}", MessageType.Heading);

            for (var blockNumber = start; blockNumber <= stop; blockNumber++) {

                var fileName = Path.Combine(Settings.Default.InputPath, $"blk{blockNumber:D5}.dat");

                _messages.Add($"File: {Path.GetFileNameWithoutExtension(fileName)}", MessageType.Heading);

                if (Path.GetFileName(fileName) == "blk00000.dat") {
                    continue;
                }

                Blocks.ClearAll();
                await Blocks.Add(fileName);

                if (Blocks[txId] != null) {
                    _messages.Add("Transaction found", MessageType.Error);
                    break;
                }
            }

            _messages.Add($"Search Complete", MessageType.Heading);
        }
    }
}

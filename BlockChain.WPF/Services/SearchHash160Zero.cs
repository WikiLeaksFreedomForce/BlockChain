using System.IO;
using System.Threading.Tasks;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {

    public class SearchHash160Zero {

        public SearchHash160Zero(MessageCollection messages){
            _messages = messages;
        }

        readonly MessageCollection _messages;

        public BlockContainer Blocks { get; } = new BlockContainer();

        public async Task Search(int start, int stop) {

            _messages.NewLine();
            _messages.Add("Finding Transactions have a TxOut Hash of 0", MessageType.Heading);

            for (var blockNumber = start; blockNumber <= stop; blockNumber++) {

                var fileName = Path.Combine(Settings.Default.InputPath, $"blk{blockNumber:D5}.dat");

                _messages.Add($"File: {Path.GetFileNameWithoutExtension(fileName)}", MessageType.Heading);

                if (Path.GetFileName(fileName) == "blk00000.dat") {
                    continue;
                }

                Blocks.ClearAll();
                await Blocks.Add(fileName);

                foreach (var block in Blocks) {
                    foreach (var transaction in block.Transactions) {

                        if (transaction.ToString() == "2c3050b7eb9b0a659a93f72e701b2cb17224af03bb42978ca3b0d2b6ffadc3d8") {
                            int j = 0;
                        }

                        foreach (var txOut in transaction.Outs){
                            if (txOut.Script.IsZero){
                                _messages.Add($"Zero: {transaction}", MessageType.Transaction);
                            }
                            else if (txOut.Script.IsOne) {
                                _messages.Add($"One: {transaction}", MessageType.Transaction);
                            }
                        }
                    }
                }
            }

            _messages.Add("Search Complete", MessageType.Heading);
        }
    }
}

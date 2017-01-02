using System.IO;
using System.Threading.Tasks;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {
    public class QueryAesKeys{

        public QueryAesKeys(MessageCollection messages){
            _messages = messages;
        }

        readonly MessageCollection _messages;

        public BlockContainer Blocks { get; } = new BlockContainer();

        public async Task Search(int start, int stop) {

            _messages.NewLine();
            _messages.Add("Searching for base64 messages", MessageType.Heading);

            for (var blockNumber = start; blockNumber <= stop; blockNumber++) {

                var fileName = Path.Combine(Settings.Default.InputPath, $"blk{blockNumber:D5}.dat");

                _messages.Add($"File: {Path.GetFileNameWithoutExtension(fileName)}", MessageType.Heading);

                if (Path.GetFileName(fileName) == "blk00000.dat") {
                    continue;
                }

                Blocks.ClearAll();
                await Blocks.Add(fileName);

                foreach (var block in Blocks){
                    foreach (var transaction in block.Transactions){
                        SearchTransaction(transaction);
                    }
                }
            }

            _messages.Add("Search complete", MessageType.Heading);
        }


        void SearchTransaction(Transaction transaction) {

            var byteArray = transaction.Outs.GetFileBytes();

            if (byteArray.Length > 240) {
                AesKeyFind.AesKeyFind.find_keys(byteArray, byteArray.Length - 240);
            }
                /*
                                    _messages.NewLine();
                                    _messages.Add(transaction.ToString());
                                    _messages.Add(text);
                                    _messages.Add(Encoding.ASCII.GetString(result));
                                    */
        }
    }
}

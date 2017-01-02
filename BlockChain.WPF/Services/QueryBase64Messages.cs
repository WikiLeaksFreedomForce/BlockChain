using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {
    public class QueryBase64Messages{

        public QueryBase64Messages(MessageCollection messages){
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

            var base64 = new List<byte>();

            foreach (var @byte in transaction.Outs.GetFileBytes()) {

                if (@byte >= 'A' && @byte <= 'Z'
                    || @byte >= '0' && @byte <= '9'
                    || @byte >= 'a' && @byte <= 'z' ||
                    @byte == '+' || @byte == '/') {

                    base64.Add(@byte);
                }
                else {

                    if (base64.Count >= 32) {
                        try {
                            var text = Encoding.ASCII.GetString(base64.ToArray());

                            // Length 64 text fields are probably Transaction IDs
                            if (text.Length != 34 && text.Length != 40 && text.Length != 64) {
                                var result = Convert.FromBase64String(text);

                                _messages.NewLine();
                                _messages.Add(transaction.ToString());
                                _messages.Add(text);
                                _messages.Add(Encoding.ASCII.GetString(result));
                            }
                        }
                        catch (Exception ex) {

                        }
                    }

                    base64.Clear();
                }
            }
        }
    }
}

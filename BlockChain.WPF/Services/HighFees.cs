using System;
using System.IO;
using System.Threading.Tasks;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {
    public class HighFees{

        public HighFees(MessageCollection messages) {
            _messages = messages;
        }

        readonly MessageCollection _messages;

        public BlockContainer Blocks { get; } = new BlockContainer();

        public BitcoinValue HighFee = new BitcoinValue(0.02m);
        public BitcoinValue NoFee = new BitcoinValue(0.0000m);

        public async Task Search(int start, int stop){

            _messages.NewLine();
            _messages.Add($"Searching for Fees greater than {HighFee}");

            for (var blockNumber = start; blockNumber <= stop; blockNumber++) {

                var fileName = Path.Combine(Settings.Default.InputPath, $"blk{blockNumber:D5}.dat");

                _messages.Add($"File: {Path.GetFileNameWithoutExtension(fileName)}");

                if (Path.GetFileName(fileName) == "blk00000.dat") {
                    continue;
                }

                Blocks.ClearAll();
                await Blocks.Add(fileName);
                Blocks.JoinInsAndOuts();

                foreach (var block in Blocks) {
                    foreach (var transaction in block.Transactions){

                        if (transaction.Ins.Amount == NoFee)
                            continue;

                        if (transaction.FeePerOut > HighFee){
                            _messages.Add($"{transaction.FeePerOut} : {transaction}");
                        }
                    }
                }
            }
        }
    }
}

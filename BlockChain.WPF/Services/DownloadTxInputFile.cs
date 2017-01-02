using System.IO;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {
    public class DownloadTxInputFile {

        public DownloadTxInputFile(BlockContainer block, MessageCollection messages){
            _block = block;
            _messages = messages;
        }

        readonly BlockContainer _block;
        readonly MessageCollection _messages;

        public void Download(string txId) {

            txId = txId.Trim(' ', '\r', '\n');

            var fileData = _block.DownloadTxInputFile(txId);

            if (fileData == null) {
                throw new InvalidDataException($"The transaction ID {txId} was not found");
            }

            // Now check for headers

            var fileName = Path.Combine(Settings.Default.OutputPath, Path.ChangeExtension(txId, fileData.Extension));

            File.Delete(fileName);
            File.WriteAllBytes(fileName, fileData.Data);

            _messages.Add($"File saved to {fileName}");
        }
    }
}

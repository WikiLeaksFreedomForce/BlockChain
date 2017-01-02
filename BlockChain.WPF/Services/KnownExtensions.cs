using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BlockChain.WPF.Messaging;
using BlockChain.WPF.Properties;

namespace BlockChain.WPF.Services {

    public class KnownExtensions{

        public KnownExtensions(MessageCollection messages) {
            _messages = messages;
        }

        readonly MessageCollection _messages;

        public BlockContainer Blocks { get; } = new BlockContainer();

        public async Task Search(int start, int stop){

            _messages.NewLine();
            _messages.Add("Searching for Known Extensions", MessageType.Heading);

            for (var blockNumber = start; blockNumber <= stop; blockNumber++) {

                var fileName = Path.Combine(Settings.Default.InputPath, $"blk{blockNumber:D5}.dat");

                _messages.Add($"File: {Path.GetFileNameWithoutExtension(fileName)}", MessageType.Heading);

                if (Path.GetFileName(fileName) == "blk00000.dat") {
                    continue;
                }

                Blocks.ClearAll();
                await Blocks.Add(fileName);

                foreach (var block in Blocks) {
                    foreach (var transaction in block.Transactions){

                        var fileData = Blocks.GetFile(transaction.ToString());

                        if (fileData == null){
                            continue;
                        }

                        var message = $"{fileData.Extension}: {transaction}";

                        switch (fileData.Extension){
                            case "dat":
                                break;

                            case "gif":
                            case "jpg":
                            case "png":
                                var data = transaction.Outs.GetFileBytes();

                                try{
                                    var imageSource = GetImageSource(data);
                                    _messages.Add(message, imageSource);
                                }
                                catch{
                                    _messages.Add(message);
                                }

                                break;

                            case "txt":
                                _messages.Add(message);
                                _messages.Add(Encoding.ASCII.GetString(fileData.Data));
                                break;

                            default:
                                _messages.Add(message);
                                break;
                        }
                    }
                }
            }

            _messages.Add("Search Complete", MessageType.Heading);
        }

        static readonly ImageConverter ImageConverter = new ImageConverter();

        public ImageSource GetImageSource(byte[] data) {
            using (Stream stream = new MemoryStream(data)){
                stream.Position = 0;

                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();

                return image;
            }
        }

        public static Bitmap GetImageFromByteArray(byte[] byteArray) {
            var bm = (Bitmap)ImageConverter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution || bm.VerticalResolution != (int)bm.VerticalResolution)) {
                // Correct a strange glitch that has been observed in the test program when converting 
                //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts" 
                //  slightly away from the nominal integer value
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f), (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }
    }
}

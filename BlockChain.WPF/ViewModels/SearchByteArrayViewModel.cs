using BlockChain.WPF.Properties;
using GalaSoft.MvvmLight;

namespace BlockChain.WPF.ViewModels {

    public class SearchByteArrayViewModel : OpenBlocksViewModel {

        public string ByteText
        {
            get { return _byteText; }
            set
            {
                Set(ref _byteText, value);

                Settings.Default.ByteText = value;
                Settings.Default.Save();
            }
        }

        string _byteText = Settings.Default.ByteText;
    }
}

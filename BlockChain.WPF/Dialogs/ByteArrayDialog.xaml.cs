using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BlockChain.WPF.ViewModels;

namespace BlockChain.WPF.Dialogs {

    /// <summary>
    /// Interaction logic for ByteArrayDialog.xaml
    /// </summary>

    public partial class ByteArrayDialog {
     
        public ByteArrayDialog() {
            InitializeComponent();

            ViewModel = new SearchByteArrayViewModel();
        }

        public SearchByteArrayViewModel ViewModel
        {
            get { return DataContext as SearchByteArrayViewModel; }
            set { DataContext = value; }
        }

        void OK_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

        void IsTextAllowed(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text) {
            var regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        void TextBoxPasting(object sender, DataObjectPastingEventArgs e) {
            if (e.DataObject.GetDataPresent(typeof(string))) {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text)) {
                    e.CancelCommand();
                }
            }
            else {
                e.CancelCommand();
            }
        }

    }
}

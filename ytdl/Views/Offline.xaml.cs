using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ytdl.Views {
    public sealed partial class Offline : Page {
        public Offline() {
            this.InitializeComponent();
        }

        private void Button_Click(object sender,RoutedEventArgs e) {
            if(Frame.CanGoBack) {
                Frame.GoBack();
            }
            else {
                Frame.Navigate(typeof(MotherPanel));
            }
        }
    }
}

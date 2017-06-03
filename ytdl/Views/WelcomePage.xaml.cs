using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ytdl.Views {
    public sealed partial class WelcomePage : Page {
        public WelcomePage() {
            this.InitializeComponent();
        }

        private void Button_Click(object sender,RoutedEventArgs e) {
            Frame.Navigate(typeof(LoginPage));
        }

        private void AppBarButton_Click(object sender,RoutedEventArgs e) {
            //jolo
            try { myPivot.SelectedIndex++; }
            catch { }
        }

        private void AppBarButton_Click_1(object sender,RoutedEventArgs e) {
            try {
                myPivot.SelectedIndex--;
            }
            catch { }
            //aghab
        }
    }
}

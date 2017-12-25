using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ytdl.Views {
    public sealed partial class WelcomePage : Page {
        public WelcomePage() {
            this.InitializeComponent();
        }
        private void Button_Click(object sender,RoutedEventArgs e) {
            Frame.Navigate(typeof(LoginPage));
			var hlp = new Classes.Helper();
			hlp.Visibility_TitleBar(true);
			hlp.ChangeTitle();
			hlp.ChangeStatus();
			Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Click Welcome.Login");
		}
	}
}

using System.Linq;
using Windows.UI.Xaml.Controls;
using ytdl.Classes;
namespace ytdl.Views {

    public sealed partial class LoginPage : Page {
        public LoginPage() {
            this.InitializeComponent();
        }
        string LoginUrl = "https://shahriar.in/app/ydm/auth/auth.php";
		string destinationUrl = "https://shahriar.in/app/ydm/auth/index.php?i=";

		private async void WebView_ContentLoading(WebView sender,WebViewContentLoadingEventArgs args) {
            try {
                string send = sender.Source.ToString().Trim();
                if(send.Length>destinationUrl.Length && send.Substring(0,destinationUrl.Length).Contains(destinationUrl)) {
                    try {
                        sender.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        string email = send.Substring(send.IndexOf(destinationUrl) + destinationUrl.Length);
						var temp = CloseHelp.Base64Decode(email);
						email = CloseHelp.Base64Decode(CloseHelp.Reverse(temp.Substring(0,temp.Length-1)));
						email = CloseHelp.Base64Encode(CloseHelp.Reverse(CloseHelp.Base64Encode("#" + email)));

						string account = await CloseHelp.DownloadPages(new System.Threading.CancellationToken(),
                            "https://shahriar.in/app/ydm/auth/getuser.php?i=" + email);
                        if(account.Substring(0,3) == "Err")
                            throw new System.Exception();
                        LocalSettingManager.SaveSetting("Account",account);
						Frame.BackStack.Remove(Frame.BackStack.Last());
						Frame.Navigate(typeof(MotherPanel));
                    }
                    catch { sender.Navigate(new System.Uri(LoginUrl)); }
                }
            }
            catch {}
        }
    }
}

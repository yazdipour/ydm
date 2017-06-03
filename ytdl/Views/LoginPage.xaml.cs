using Windows.UI.Xaml.Controls;
using ytdl.Classes;
namespace ytdl.Views {

    public sealed partial class LoginPage : Page {
        public LoginPage() {
            this.InitializeComponent();
        }
        string LoginUrl = "http://shahriar.in/app/ytdlr/auth/logout.php";

        private async void WebView_ContentLoading(WebView sender,WebViewContentLoadingEventArgs args) {
            try {
                string url = "http://shahriar.in/app/ytdlr/auth/index.php?i=";
                string send = sender.Source.ToString().Trim();
                if(send.Length>url.Length &&send.Substring(0,url.Length).Contains(url)) {
                    try {
                        sender.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        string email = send.Substring(send.IndexOf(url) + url.Length);
                        if(email.Contains("#"))
                            email = email.Substring(0,email.Length - 1);
                        email = CloseHelp.Base64Decode(email);
                        email = email.Substring(0,email.Length - 1);
                        //email = CloseHelp.Reverse(email);
                        //email = CloseHelp.Base64Decode(email); // Gives The Email
                        email = CloseHelp.Base64Encode(email); // ready for GetUser
                        string account = await CloseHelp.DownloadPages(new System.Threading.CancellationToken(),
                            "http://shahriar.in/app/ytdlr/auth/getuser.php?i=" + email);
                        if(account.Substring(0,4) == "Err:")
                            throw new System.Exception();
                        LocalSettingManager.SaveSetting("Account",account);
                        Frame.Navigate(typeof(MotherPanel));
                    }
                    catch { sender.Navigate(new System.Uri(LoginUrl)); }
                }
            }
            catch {}
        }
    }
}

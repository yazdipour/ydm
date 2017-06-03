using Windows.UI.Xaml.Controls;
using System;
using Windows.ApplicationModel.Email;
using ytdl.Classes;
using ytdl.Models;
using Windows.UI.Popups;

namespace ytdl.Views {
    public sealed partial class UserPanel : Page {
        public UserPanel() {
            this.InitializeComponent();
            Img = App.Usr.Img;
            leftDay = App.Usr.leftDay + 1;
            mailTxt = App.Usr.Email;
        }
        User usr = new User();
        string Img = "",mailTxt;
        int leftDay = 0;
        private async void Logout_Click(object sender,Windows.UI.Xaml.RoutedEventArgs e) {
            string msg = "آیا قصد خروج از این حساب را دارید؟ (ریستارت برنامه)";
            var dialog = new MessageDialog(msg);
            dialog.Commands.Add(new UICommand("بیخیال"));
            dialog.Commands.Add(new UICommand("باشه",OnOKButtonClicked));
            await dialog.ShowAsync();
        }

        private async void OnOKButtonClicked(IUICommand command) {
            LocalSettingManager.RemoveSetting("Account");
            LocalSettingManager.RemoveSetting("DI");
            await CloseHelp.DownloadPages(new System.Threading.CancellationToken(false),"http://shahriar.in/app/ytdlr/auth/logout.php");
            Windows.UI.Xaml.Application.Current.Exit();
        }

        private async void AppBarButton_Click(object sender,Windows.UI.Xaml.RoutedEventArgs e) {
            MotherPanel.StaticRing.Visibility = Windows.UI.Xaml.Visibility.Visible;
            //refresh
            string get = Newtonsoft.Json.JsonConvert.SerializeObject(new string[] { App.Usr.Id.ToString(),App.Usr.Email });
            get = CloseHelp.Base64Encode(CloseHelp.Reverse(CloseHelp.Base64Encode(get)));
            string url = "http://shahriar.in/app/ytdlr/dl/getdate.php?i=" + get;
            url = await CloseHelp.DownloadPages(new System.Threading.CancellationToken(),url);
            try {
                var arr = url.Split('|');
                App.Usr.leftDay = Convert.ToInt32(arr[1]);
                if(App.Usr.leftDay < 0)
                    App.Usr.leftDay = -1;
                leftDay = App.Usr.leftDay + 1;
                leftDayText.Text = leftDay.ToString();
                App.Today = Convert.ToInt32(arr[0]);
            }
            catch { }

            MotherPanel.StaticRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        #region BTN
        private async void Err1_Click(object sender,Windows.UI.Xaml.RoutedEventArgs e) {
            var mail = new EmailMessage();
            mail.To.Add(new EmailRecipient { Address = "app@shahriar.in" });
            mail.Subject = "مشکل در شارژ یوتیوب دانلودر";
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void Err2_Click(object sender,Windows.UI.Xaml.RoutedEventArgs e) {
            var mail = new EmailMessage();
            mail.To.Add(new EmailRecipient { Address = "app@shahriar.in" });
            mail.Subject = "مشکل در برنامه یوتیوب دانلودر";
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private void Charge_Click(object sender,Windows.UI.Xaml.RoutedEventArgs e) {
            Frame.Navigate(typeof(PayPanel));
        }
        #endregion

    }
}

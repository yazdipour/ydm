using Windows.UI.Xaml.Controls;
using ytdl.Classes;

namespace ytdl.Views {
    public sealed partial class PayPanel : Page {
        public PayPanel() {
            this.InitializeComponent();
        }
        string LoginUrl= "http://app.shahriar.in/ytdlr/pay/?e="+CloseHelp.Base64Encode(App.Usr.Email);

    }
}

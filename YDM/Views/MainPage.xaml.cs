using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using YDM.Share;

namespace YDM.Views
{
    public partial class MainPage : ContentPage
    {
        private string BASE_URL = "";
        ApiHandler apiHandler;
        public MainPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            buildLabel.Text = $"YDM\n{AppInfo.VersionString}";
            BASE_URL = Preferences.Get("baseurl", "https://ydm.herokuapp.com");
            apiHandler = new ApiHandler(BASE_URL);
        }

        void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            //throw new NotImplementedException();

        }

        async void Settings_Clicked(object sender, System.EventArgs e)
        {
            var input = await MaterialDialog.Instance.InputAsync(
                "Settings",
                "Put BaseUrl for your server:",
                BASE_URL);
            if (input?.Length > 5) Preferences.Set("baseurl", input);
        }




        async void Copy_Clicked(object sender, System.EventArgs e)
            => entry.Text = await Clipboard.GetTextAsync();


        async void Get_Clicked(object sender, System.EventArgs e)
        {
            await MaterialDialog.Instance.SnackbarAsync(message: "This is a snackbar.",
                    actionButtonText: "Got It",
                    msDuration: 3000);
        }


        async void Search_Clicked(object sender, System.EventArgs e) => await Navigation.PushAsync(new SearchPage("Search"));

        async void Playlist_Clicked(object sender, System.EventArgs e) => await Navigation.PushAsync(new SearchPage("PlayList"));
    }
}

using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using YDM.Share;
using System.Diagnostics;
using Newtonsoft.Json.Serialization;

namespace YDM.Views
{
    public partial class MainPage : ContentPage
    {
        private string BASE_URL = "";
        ApiHandler apiHandler;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            buildLabel.Text = $"YDM\n{AppInfo.VersionString}";
            BASE_URL = Preferences.Get("baseurl", "https://ydm.herokuapp.com");
            apiHandler = new ApiHandler(BASE_URL);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await apiHandler.LoadVideoHistory();
            listView.ItemsSource = apiHandler.DownloadHistory;
        }

        void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {

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
            var getBtn = sender as Button;
            getBtn.IsEnabled = false;
            await MaterialDialog.Instance.SnackbarAsync("Please Wait...", 1000);
            try
            {
                var videoUrl = entry.Text.Trim();
                if (videoUrl.Length < "_JNeiGbAgL4".Length) throw new Exception("Video Url is invalid");
                var result = await apiHandler.Api.GetAvailableVideoLink(videoUrl, apiHandler.DownloadHistory);
                var msg = (result.Info.Title.Length > 20) ? result.Info.Title.Substring(0, 20) + "..." : result.Info.Title;
                await MaterialDialog.Instance.SnackbarAsync(message: "Added: " + msg, actionButtonText: "Got It", msDuration: 3000);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: " + ex.Message, msDuration: 3000);
            }
            finally
            {
                getBtn.IsEnabled = true;
            }
        }

        async void Search_Clicked(object sender, System.EventArgs e) => await Navigation.PushAsync(new SearchPage("Search"));

        async void Playlist_Clicked(object sender, System.EventArgs e) => await Navigation.PushAsync(new SearchPage("PlayList"));
    }
}

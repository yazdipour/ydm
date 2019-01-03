using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using YDM.Share;
using System.Diagnostics;
using YDM.Share.Models;

namespace YDM.Views
{
    public partial class MainPage : ContentPage
    {
        ApiHandler apiHandler;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            apiHandler = new ApiHandler();
            if (apiHandler.DownloadHistory?.Count == 0)
            {
                await apiHandler.LoadVideoHistory();
                listView.ItemsSource = apiHandler.DownloadHistory;
            }
            countLabel.Text = apiHandler.DownloadHistory.Count.ToString();
            buildLabel.Text = $"YDM\n{AppInfo.VersionString}";
        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DownloadedItems item)
                await Navigation.PushAsync(new WatchPage(Title, item, apiHandler));
        }

        async void Settings_Clicked(object sender, EventArgs e)
        {
            var input = await MaterialDialog.Instance.InputAsync(
                "Settings",
                "Set server address:",
                apiHandler.BASE_URL.ToString());
            if (input?.Length > 5)
            {
                apiHandler.BASE_URL.SetBaseUrl(input);
                await apiHandler.InitApi();
            }
        }

        async void About_Clicked(object sender, EventArgs e)
        {
            await MaterialDialog.Instance.AlertAsync(
                  title: "About YDM",
                  message: "YDM is an Open source Youtube Client-Server Application.\n\n" +
                           "Dev: Shahriar Yazdipour\n\n" +
                           "Source: github.com/yazdipour/YDM",
                  acknowledgementText: "Aha!");
        }

        async void Copy_Clicked(object sender, System.EventArgs e)
            => entry.Text = await Clipboard.GetTextAsync();

        async void Get_Clicked(object sender, System.EventArgs e)
        {
            var getBtn = sender as Button;
            getBtn.IsEnabled = false;
            entry.HasError = false;
            try
            {
                var videoUrl = entry.Text.Trim();
                if (videoUrl.Length < "_JNeiGbAgL4".Length) throw new Exception("Video Url is invalid");
                await MaterialDialog.Instance.SnackbarAsync(message: "Please Wait...", msDuration: 2000);
                var result = await apiHandler.Api.GetAvailableVideoLink(videoUrl, apiHandler.DownloadHistory);
                var msg = (result.Info.Title.Length > 20) ? result.Info.Title.Substring(0, 20) + "..." : result.Info.Title;
                await MaterialDialog.Instance.SnackbarAsync(message: "Added: " + msg, actionButtonText: "Got It", msDuration: 3000);
                countLabel.Text = apiHandler.DownloadHistory.Count.ToString();
            }
            catch (Exception ex)
            {
                entry.HasError = true;
                Debug.WriteLine(ex.Message);
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: " + ex.Message, msDuration: 2000);
            }
            finally
            {
                getBtn.IsEnabled = true;
            }
        }

        void OnDelete(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menu)
                if (menu.CommandParameter is DownloadedItems item)
                {
                    apiHandler.RemoveHistoryItem(item);
                    countLabel.Text = apiHandler.DownloadHistory.Count.ToString();
                }
        }

        async void Search_Clicked(object sender, System.EventArgs e) => await Navigation.PushAsync(new SearchPage("Search", apiHandler));

        async void Playlist_Clicked(object sender, System.EventArgs e) => await Navigation.PushAsync(new SearchPage("PlayList", apiHandler));
    }
}

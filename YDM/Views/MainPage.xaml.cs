using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using System.Diagnostics;
using YDM.Share.Models;
using YDM.ViewModels;

namespace YDM.Views
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = viewModel = new MainPageViewModel();
        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is VideoItem item)
                await Navigation.PushAsync(new WatchPage(Title, item, viewModel.apiHandler));
        }

        async void Settings_Clicked(object sender, EventArgs e)
        {
            var input = await MaterialDialog.Instance.InputAsync("Settings", "Set server address:", viewModel.apiHandler.BASE_URL.ToString());
            if (input?.Length < 5) return;
            viewModel.apiHandler.BASE_URL.SetBaseUrl(input);
            await viewModel.apiHandler.InitApi();
        }

        async void About_Clicked(object sender, EventArgs e)
         => await MaterialDialog.Instance.AlertAsync(
                  title: "About YDM", acknowledgementText: "Aha!",
                  message: "YDM is an Open source Youtube Client-Server Application.\n\n" +
                           "Dev: Shahriar Yazdipour\n\nSource: github.com/yazdipour/YDM");

        async void Get_Clicked(object sender, System.EventArgs e)
        {
            var getBtn = sender as Button;
            getBtn.IsEnabled = false;
            entry.HasError = false;
            try
            {
                var videoUrl = entry.Text.Trim();
                if (videoUrl.Length < "_JNeiGbAgL4".Length) throw new Exception("Video Url is invalid");
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Please Wait..."))
                {
                    var result = await viewModel.apiHandler.Api.GetAvailableVideoLink(videoUrl, viewModel.apiHandler.DownloadHistory);
                    var msg = (result.Info.Title.Length > 20) ? result.Info.Title.Substring(0, 20) + "..." : result.Info.Title;
                    await MaterialDialog.Instance.SnackbarAsync(message: "Added: " + msg, actionButtonText: "Got It", msDuration: 3000);
                }
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
            if (sender is MenuItem menu && menu.CommandParameter is VideoItem item)
                viewModel.apiHandler.RemoveHistoryItem(item);
        }

        #region Button Clicked
        async void Copy_Clicked(object sender, System.EventArgs e) => entry.Text = await Clipboard.GetTextAsync();

        async void Search_Clicked(object sender, System.EventArgs e) => await Navigation.PushAsync(new SearchPage("Search", viewModel.apiHandler));

        async void Playlist_Clicked(object sender, System.EventArgs e) => await Navigation.PushAsync(new SearchPage("PlayList", viewModel.apiHandler));
        #endregion
    }
}

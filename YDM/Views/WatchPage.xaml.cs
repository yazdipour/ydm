
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using YDM.Share;
using YDM.Share.Models;
using System.Linq;

namespace YDM.Views
{
    public partial class WatchPage : ContentPage
    {
        private readonly ApiHandler apiHandler;
        private readonly string parentPage;
        private readonly VideoItem item;
        private string SourceUrl => $"https://www.youtube.com/watch?v={item?.Id}";
        private DownloadLink[] linkItems;

        public WatchPage(string parent, VideoItem item, ApiHandler api)
        {
            InitializeComponent();
            BindingContext = this;
            apiHandler = api;
            parentPage = parent;
            this.item = item;
            Title = item.Title;
            TitleLabel.Text = item.Title;
            DurationLabel.Text = item.Duration;
            GetDownloadableLinks();
        }

        private async void GetDownloadableLinks()
        {
            try
            {
                linkItems = await apiHandler.Api.GetDownloadableLinks(item.Id);
                var videoUrl = linkItems[0]?.Url;
                videoUrl = (await apiHandler.GetRedirectedUri(videoUrl)).AbsoluteUri;
                webView.Source = new HtmlWebViewSource
                {
                    Html = $"<html> <body background=\"black\"> <video poster=\"{item.Image}\"controls> <source src=\"{videoUrl}\" type=\"video/mp4\"> </video> </body> </html>"
                };
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message, "Close");
            }
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (sender is Button btn)
            {
                string content = btn.Text.ToLower();
                if (content.Contains("source"))
                {
                    if (content.Contains("copy"))
                    {
                        await Xamarin.Essentials.Clipboard.SetTextAsync(SourceUrl);
                        await MaterialDialog.Instance.SnackbarAsync(message: "Copied!", msDuration: MaterialSnackbar.DurationShort);
                    }
                    else Device.OpenUri(new Uri(SourceUrl ?? "https://www.youtube.com"));
                }
                else if (content.Contains("download"))
                {
                    var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select Video Quality",
                        actions: linkItems?.Select((DownloadLink arg) => arg?.Subtext).ToArray());
                    if (content.Contains("copy"))
                    {
                        await Xamarin.Essentials.Clipboard.SetTextAsync(apiHandler.Api.GetDownloadLink(item?.Id, result.ToString()));
                        await MaterialDialog.Instance.SnackbarAsync(message: "Copied!", msDuration: MaterialSnackbar.DurationShort);
                    }
                    else await DisplayAlert("Sorry!", "This feature, is not developed yet!", "Ok");
                    //Help! https://github.com/xamarin/recipes/tree/master/Recipes/cross-platform/networking/download_progress
                }

            }
        }
    }
}

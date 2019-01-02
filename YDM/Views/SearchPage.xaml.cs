using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using YDM.Share.Models;
using YDM.Share;

namespace YDM.Views
{
    public partial class SearchPage : ContentPage
    {
        private readonly ApiHandler apiHandler;

        public SearchPage() => InitializeComponent();
        public SearchPage(string v, ApiHandler apiHandler)
        {
            InitializeComponent();
            Title = v;
            this.apiHandler = apiHandler;
        }


        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DownloadedItems item)
                await Navigation.PushAsync(new WatchPage(Title, item, apiHandler));
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
                var q = entry.Text.Trim();
                if (q.Length < 3) throw new Exception("Too Short!");
                await MaterialDialog.Instance.SnackbarAsync(message: "Please Wait...", msDuration: 2000);
                listView.ItemsSource = Title == "Search" ? await apiHandler.Api.Search(q, 20) : await apiHandler.Api.GetPlayListItems(q);
            }
            catch (Exception ex)
            {
                entry.HasError = true;
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: " + ex.Message, msDuration: 2000);
            }
            finally
            {
                getBtn.IsEnabled = true;
            }
        }

    }
}

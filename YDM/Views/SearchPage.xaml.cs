using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace YDM.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage() => InitializeComponent();
        public SearchPage(string v)
        {
            InitializeComponent();
            Title = v;
        }

        void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            //throw new NotImplementedException();

        }

        async void Copy_Clicked(object sender, System.EventArgs e)
            => entry.Text = await Clipboard.GetTextAsync();


        async void Get_Clicked(object sender, System.EventArgs e)
        {
            await MaterialDialog.Instance.SnackbarAsync(
                    message: "This is a snackbar.",
                    actionButtonText: "Got It",
                    msDuration: 3000);
            searchTitle.Text = entry.Text?.Trim();
        }

    }
}

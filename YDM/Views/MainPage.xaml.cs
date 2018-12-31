using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace YDM.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //await MaterialDialog.Instance.SnackbarAsync(message: "This is a snackbar.",
            //actionButtonText: "Got It",
            //msDuration: 3000);
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}

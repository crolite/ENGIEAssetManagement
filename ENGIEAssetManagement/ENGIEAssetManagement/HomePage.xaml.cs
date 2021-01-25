using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ENGIEAssetManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        public HomePage()
        {
            InitializeComponent();
        }

        private void QRScan_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new QRScanner());
        }


        private void Logout_Clicked(object sender, EventArgs e)
        {
            // return to root main page (the login page) workaround as Navigation.PopToRootAsync(); isn't working
            App.Current.MainPage = new NavigationPage(new LoginPage());
           
        }
    }
}
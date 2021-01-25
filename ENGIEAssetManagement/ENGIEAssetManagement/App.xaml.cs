using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIEAssetManagement
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage()); // set LoginPage as the root page of the app
       
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

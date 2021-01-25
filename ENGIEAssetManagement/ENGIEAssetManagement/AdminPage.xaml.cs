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
    public partial class AdminPage : ContentPage
    {
        FirebaseConnector fc = new FirebaseConnector();

        public AdminPage()
        {
            InitializeComponent();
        }

        private async void listUsers_Clicked(object sender, EventArgs e)
        {
            errorLabel.Text = "";

            try
            {
                // get all users and show in list
                var allUsers = await fc.GetAllUsers();
                listUsers.ItemsSource = allUsers;
            }
            catch (Firebase.Database.FirebaseException)
            {
                errorLabel.Text = "Error: Firebase Database Exception - please check your internet connection";
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }


        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            // return to root main page (the login page) workaround as Navigation.PopToRootAsync(); isn't working
            App.Current.MainPage = new NavigationPage(new LoginPage());

        }
    }
}
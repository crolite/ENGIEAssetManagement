
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ENGIEAssetManagement.Models;

namespace ENGIEAssetManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void HandleLogin(object sender, EventArgs e)
        {
            // disable button to prevent repeated login
            loginButton.IsEnabled = false;

            // get details
            string userAttempt = entryUser.Text;
            string passAttempt = entryPass.Text;

            try
            {
                // I think this is handled automatically anyway
                if (string.IsNullOrEmpty(userAttempt) || string.IsNullOrEmpty(passAttempt))
                {
                    throw new Exception("Error: Username or Password input is invalid or empty.");
                }

                FirebaseConnector fc = new FirebaseConnector();

                var user = await fc.GetUser(userAttempt);
                // if user exists
                if (user != null)
                {
                    // does the password match when hashing? first get salt and hashed password from saved data.
                    string savedSaltAndPass = user.HashedPassword; // len 56: first 24 salt, next 32 hash
                    string salt = savedSaltAndPass.Substring(0, 24);
                    string hashedPass = savedSaltAndPass.Substring(24, 32);

                    // hash the input using saved salt and check result with saved data
                    string hashedInput = HashingUtility.GenerateHash(passAttempt, salt);
                    if (String.Equals(hashedInput, hashedPass)) // if password correct ...
                    {

                        if(user.Admin)
                        {
                            await Navigation.PushModalAsync(new AdminPage());
                        } 
                        else
                        {
                            // user so send to home page (where can choose to scan QR, generate, logout)
                            await Navigation.PushModalAsync(new HomePage());
                        }       
                    }
                    else
                    {
                        throw new Exception("Error: Password incorrect.");
                    }
                }
                else
                {
                    throw new Exception("Error: Username not present in database.");
                }

            }
            catch (Firebase.Database.FirebaseException)
            {
                loginResult.Text = "Error: Firebase Database Exception - please check your internet connection";
            }
            catch (Exception ex)
            {
                loginResult.Text = ex.Message;
            }
            finally
            {
                loginButton.IsEnabled = true;
            }

        }

        private void GoToCreateAccount(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CreateAccountPage());
        }
    }
}


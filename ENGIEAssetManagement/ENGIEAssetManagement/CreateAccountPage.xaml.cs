using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ENGIEAssetManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccountPage : ContentPage
    {
        public CreateAccountPage()
        {
            InitializeComponent();
        }

        private void GoToLogin(object sender, EventArgs e)
        {
            // we can only get here from login, so remove this page and go back
            Navigation.PopModalAsync();
        }

        private async void HandleCreateAccount(object sender, EventArgs e)
        {
            // disable button to prevent repeated account creation
            createAccountButton.IsEnabled = false;
            
            string userInput = entryUser.Text;
            string passInput = entryPass.Text;

            try
            {
                if (string.IsNullOrEmpty(userInput) || string.IsNullOrEmpty(passInput))
                {
                    throw new Exception("Error: Username or Password input is invalid or empty.");
                }

                // is username valid?
                if (!Regex.IsMatch(userInput, "^[a-zA-Z0-9@#$!%^&+=]{5,12}$")) 
                {
                    throw new Exception("Username must be between 5 and 15 characters, and can contain: lower/uppercase letters, digits 0-9, and symbols '@, #, $, !, %, ^, &, +, ='");
                }
                // is password invalid?
                else if (!Regex.IsMatch(passInput, "^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[@#$!%^&+=])[a-zA-Z0-9@#$!%^&+=]{8,20}$"))
                {
                    throw new Exception("Password must be between 8 and 20 characters, and must contain lower and uppercase letters, digits 0-9, symbols '@, #, $, !, %, ^, &, +, =', and atleast one of each.");
                }
                // then the details are valid
                else
                {
                    
                    // hash the password
                    string salt = HashingUtility.GenerateSalt();
                    string passwordHash = HashingUtility.GenerateHash(passInput, salt);
                    string passwordToStore = salt + passwordHash; // the form to store password in (prepend hash with salt)

                    
                    // add account (email, passwordToStore) to database
                    FirebaseConnector fc = new FirebaseConnector();

                    // check if username not already present
                    if (await fc.GetUser(userInput) == null)
                    {
                        // if not present, insert new data
                        await fc.AddUser(userInput, passwordToStore);

                        await Navigation.PushModalAsync(new HomePage()); // take to homepage
                    }
                    else
                    {
                        throw new Exception("Error: User already exists with this username.");
                    }

                }

            }
            catch (Firebase.Database.FirebaseException)
            {
                createAccResult.Text = "Error: Firebase Database Exception - please check your internet connection";
            }
            catch (Exception ex)
            {
                createAccResult.Text = ex.Message;
            }
            finally
            {
                createAccountButton.IsEnabled = true;
            }
 

        }
       
    }

}
      
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Firebase.Database;
using Firebase.Database.Query;
using ENGIEAssetManagement.Models;


namespace ENGIEAssetManagementTests
{
    [TestClass]
    public class LoginPageTests
    {
        /*
         * setup dummy user (copied code from FirebaseConnectorTests)
         * recreate login code
         * see if can login, fail correctly etc
        */


        static ENGIEAssetManagement.FirebaseConnector fc = new ENGIEAssetManagement.FirebaseConnector();

        // setup dummy user
        [ClassInitialize]
        public static async Task AddUserForTesting(TestContext tc)
        {
            // this is code from create account, to hash our password before we make our dummy user
            // hash the password
            string salt = ENGIEAssetManagement.HashingUtility.GenerateSalt();
            string passwordHash = ENGIEAssetManagement.HashingUtility.GenerateHash("PassWord123!", salt);
            string passwordToStore = salt + passwordHash; 

            // create our dummy user
            await fc.AddUser("LPTestUser", passwordToStore);
        }

        // after we test, delete our dummy user
        [ClassCleanup]
        public static async Task DeleteOurTestingUser()
        {

            // This code is not needed in actual project, but included here so when testing add user, can delete them after.

            // connect to firebase DB (this exists in fc instantiated above, but reconnect so don't have to make public in FirebaseConnector)
            FirebaseClient firebaseDatabase = new FirebaseClient("https://engie-asset-manager-default-rtdb.europe-west1.firebasedatabase.app/");

            // find the person
            var userToDelete = (await firebaseDatabase
                .Child("Users")
                .OnceAsync<User>
                ()).Where(a => a.Object.Username == "LPTestUser").FirstOrDefault();

            await firebaseDatabase.Child("Users").Child(userToDelete.Key).DeleteAsync();
        }

        // the same login functionality as in LoginPage.xaml.cs - but return strings for testing
        private static async Task<string> Login(string userAttempt, string passAttempt)
        {
        
            try
            {
                // I think this is handled automatically anyway
                if (string.IsNullOrEmpty(userAttempt) || string.IsNullOrEmpty(passAttempt))
                {
                    return "exception: null";
                }

                
                var user = await fc.GetUser(userAttempt);
                // if user exists
                if (user != null)
                {
                    // does the password match when hashing? first get salt and hashed password from saved data.
                    string savedSaltAndPass = user.HashedPassword; // len 56: first 24 salt, next 32 hash
                    string salt = savedSaltAndPass.Substring(0, 24);
                    string hashedPass = savedSaltAndPass.Substring(24, 32);

                    // hash the input using saved salt and check result with saved data
                    string hashedInput = ENGIEAssetManagement.HashingUtility.GenerateHash(passAttempt, salt);
                    if (String.Equals(hashedInput, hashedPass)) // if password correct ...
                    {

                        if (user.Admin)
                        {
                            return "admin";
                        }
                        else
                        {
         
                            return "user";
                        }
                    }
                    else
                    {
                        return "exception: password"; // password wrong
                    }
                }
                else
                {
                    return "exception: username"; // user not present
                }

            }
            catch (Exception ex)
            {
                // shouldn't reach this in testing?
                return ex.Message;
            }
            
        }


        // test the functionality

        [TestMethod]
        public async Task NullUsernameGivesCorrectException()
        {
            Assert.AreEqual("exception: null", await Login(null, "PassWord23!")); // also incorrect pass, but this shouldn't be identified in this method
        }

        [TestMethod]
        public async Task NullPasswordGivesCorrectException()
        {
            Assert.AreEqual("exception: null", await Login(".", null)); // also non-present user, but this shouldn't be identified in this method
        }

        [TestMethod]
        public async Task CorrectLoginAsUserWorks()
        {
            Assert.AreEqual("user", await Login("LPTestUser", "PassWord123!")); 
        }

        [TestMethod]
        public async Task CorrectLoginAsUserDoesNOTReturnAdmin()
        {
            Assert.AreNotEqual("admin", await Login("LPTestUser", "PassWord123!"));
        }

        [TestMethod]
        public async Task CorrectLoginAsAdminWorks()
        {
            // this is assuming my test admin account, "TestAdmin", is still present
            // admin property difficult to test normally as can only be set to true within the Firebase
            Assert.AreEqual("admin", await Login("TestAdmin", "TestingPass123!"));
        }

        [TestMethod]
        public async Task IncorrectPasswordLoginBehavesCorrectly()
        {
            Assert.AreEqual("exception: password", await Login("LPTestUser", "PassWord23!"));
        }

        [TestMethod]
        public async Task NonPresentUsernameLoginBehavesCorrectly()
        {
            Assert.AreEqual("exception: username", await Login(".", "PassWord123!"));
        }

    }
}

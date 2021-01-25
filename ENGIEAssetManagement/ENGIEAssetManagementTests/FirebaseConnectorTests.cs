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
    public class FirebaseConnectorTests
    {

        // create instance of firebase connector class to test
        static ENGIEAssetManagement.FirebaseConnector fc = new ENGIEAssetManagement.FirebaseConnector();


        // for forms files
        [TestMethod] 
        public async Task GetFilesGetsAllFiles()
        {
            // GetFile returns null if file not found
            Assert.IsTrue(await fc.GetFile("EmergencyLightingFullSheet.pdf") != null);
            Assert.IsTrue(await fc.GetFile("RCDElectricalTest.pdf") != null);
            Assert.IsTrue(await fc.GetFile("RetroJobSheet.docx") != null);
            Assert.IsTrue(await fc.GetFile("FixedApplianceTestingSheet.xlsx") != null);
            Assert.IsTrue(await fc.GetFile("GasInspectionCertificate.jpeg") != null);
        }

        [TestMethod]
        public async Task GetFilesReturnsNullForInvalidFile()
        {
            // GetFile returns null if file not found
            Assert.IsNull(await fc.GetFile("etroJobSheet.docx"));
            Assert.IsNull(await fc.GetFile("1234"));
            Assert.IsNull(await fc.GetFile(null));
            Assert.IsNull(await fc.GetFile("bdawubiadwbadwbowadbwadbuowadbuodwabadwfebhfehuwawhbk"));
        }


        // operations on DB

        // before we test
        [ClassInitialize]
        public static async Task AddUserForTesting(TestContext tc)
        {
            // non-hashed password doesn't matter, hashing is tested in HashingUtilityTests
            await fc.AddUser("FCTestUser", "this should not be here");
        }

        // after we test
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
                ()).Where(a => a.Object.Username == "FCTestUser").FirstOrDefault();

            await firebaseDatabase.Child("Users").Child(userToDelete.Key).DeleteAsync();
        }
        

        // our db test methods ....

        [TestMethod]
        public async Task GetUserReturnsUserForAddedUser()
        {
            // is our test user there
            Assert.IsTrue(await fc.GetUser("FCTestUser") != null);

            // is it a User
            Assert.IsTrue((await fc.GetUser("FCTestUser")).GetType() == typeof(User));

            // we could test GetAllUsers, but don't need to as GetUser uses this to get a list of all users and search for specific user.
            
        }

        [TestMethod]
        public async Task GetUserReturnsNullForInvalidUser()
        {
            // make sure returns null if user not present
            Assert.IsNull(await fc.GetUser("abcdefghijklmnop")); // this user couldnt be made in normal operation as exceeds 12 char limit
        }

        [TestMethod]
        public async Task GetUserIsNullForNullInput()
        {
            Assert.IsNull(await fc.GetUser(null));
        }

    }
}

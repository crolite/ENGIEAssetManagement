using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Storage;
using Firebase.Database;
using Firebase.Database.Query;
using ENGIEAssetManagement.Models;

namespace ENGIEAssetManagement
{
    public class FirebaseConnector
    {
        FirebaseStorage firebaseStorage = new FirebaseStorage("engie-asset-manager.appspot.com"); // form storage
        FirebaseClient firebaseDatabase = new FirebaseClient("https://engie-asset-manager-default-rtdb.europe-west1.firebasedatabase.app/"); // user db

        // for use with forms
        public async Task<string> GetFile(string FileName)
        {
            try
            {
                //Return the download link for the file if it is found in the ENGIE Files folder of the firebase storage
                return await firebaseStorage
                .Child("ENGIE Files")
                .Child(FileName)
                .GetDownloadUrlAsync();

            }
            catch(FirebaseStorageException)
            {
                //Return null if the specified file isn't found
                return null;
            }
        }

        // operations on database
        public async Task AddUser(string username, string hashedPassword)
        {
            await firebaseDatabase
                .Child("Users")
                .PostAsync(new User() { Username = username, HashedPassword = hashedPassword });  // Admin property doesn't have to be set, has default value of false
        }


        public async Task<List<User>> GetAllUsers() // seperated into different method as used in admin
        {
            return (await firebaseDatabase
                .Child("Users")
                .OnceAsync<User>()).Select(item => new User
                 {
                     Username = item.Object.Username,
                     HashedPassword = item.Object.HashedPassword,
                     Admin = item.Object.Admin
                 }).ToList();

        }


        public async Task<User> GetUser(string username)
        {

            // get all people - seems inefficient
            var allUsers = await GetAllUsers();

            // find the person (username is unique identifier)
            return allUsers.Where(a => a.Username == username).FirstOrDefault();

        }

        /* We probably don't need these methods, they will need updating to work if we do (added 'admin' property to user model since)
         
        public async Task UpdateUser(string username, string hashedPassword) 
        {
            // find the person
            var userToUpdate = (await firebaseDatabase
                .Child("Users")
                .OnceAsync<User>()).Where(a => a.Object.Username == username).FirstOrDefault();

            // add new person with same ID (overwrite)
            await firebaseDatabase
                .Child("Users")
                .Child(userToUpdate.Key)
                .PutAsync(new User() { Username = username, HashedPassword = hashedPassword });

         } */

        public async Task DeleteUser(string username)
        {
            // find the person
            var userToDelete = (await firebaseDatabase
                .Child("Users")
                .OnceAsync<User>
                ()).Where(a => a.Object.Username == username).FirstOrDefault();

            await firebaseDatabase.Child("Users").Child(userToDelete.Key).DeleteAsync();

        }

       

    }
}

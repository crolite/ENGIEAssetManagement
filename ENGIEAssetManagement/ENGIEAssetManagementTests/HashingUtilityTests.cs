using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENGIEAssetManagement;

namespace ENGIEAssetManagementTests
{
    [TestClass]
    public class HashingUtilityTests
    {

        // Methods involved in Account Creation (Creating a hashed password)
        string testPassword = "PassWord123!";
        string testSalt = HashingUtility.GenerateSalt();

        [TestMethod]
        public void GenerateSaltGeneratesCorrectLength()
        {
            Assert.AreEqual(24, HashingUtility.GenerateSalt().Length);
        }

        [TestMethod]
        public void GenerateSaltGeneratesDifferentSalts()
        {
            // not a great test, but just to ensure it isn't exactly the same every time
            // gen 3 salts
            var saltA = HashingUtility.GenerateSalt();
            var saltB = HashingUtility.GenerateSalt();
            var saltC = HashingUtility.GenerateSalt();

            // ensure none are the same
            Assert.IsTrue(saltA != saltB &&
                           saltB != saltC &&
                           saltC != saltA);

        }

        [TestMethod]
        public void GenerateHashGeneratesCorrectLength()
        {
            Assert.AreEqual(32, HashingUtility.GenerateHash(testPassword, testSalt).Length);
            Assert.AreEqual(32, HashingUtility.GenerateHash(".", HashingUtility.GenerateSalt()).Length);
            Assert.AreEqual(32, HashingUtility.GenerateHash("iadwinwdapndawpnidawni[qrfqnpifqnipqfnqvnpqvipqvinpqnipqfwpniqfwinfqw", HashingUtility.GenerateSalt()).Length);
        }

        [TestMethod]
        public void GenerateHash_SameInputSameSaltSameResult()
        {
            Assert.AreEqual(HashingUtility.GenerateHash(testPassword, testSalt), HashingUtility.GenerateHash(testPassword, testSalt));
        }

        [TestMethod]
        public void GenerateHash_SameInputDifferentSaltDifferentResult()
        {
            Assert.AreNotEqual(HashingUtility.GenerateHash(testPassword, testSalt), HashingUtility.GenerateHash(testPassword, HashingUtility.GenerateSalt()));
        }



        // Methods involved in Logging In (Checking an attempt against a saved hashed password)
        [TestMethod]
        public void CorrectPasswordLoginWorks()
        {
            string testHashedPassword = testSalt + HashingUtility.GenerateHash(testPassword, testSalt); // create a hashed password in the same way that Create Account does

            // break down saved password in same way Login Page does
            string salt = testHashedPassword.Substring(0, 24);
            string hashedPass = testHashedPassword.Substring(24, 32);

            string inputAttempt = testPassword; // we enter the correct password

            // hash the input using saved salt and check result with saved data
            string hashedInput = HashingUtility.GenerateHash(inputAttempt, salt);

            // assert that when we hash the input, it correctly matches saved data
            Assert.AreEqual(hashedInput, hashedPass);
        }

       
        [TestMethod]
        public void IncorrectPasswordLoginFails()
        {
            string testHashedPassword = testSalt + HashingUtility.GenerateHash(testPassword, testSalt); // create a hashed password in the same way that Create Account does

            // break down saved password in same way Login Page does 
            string salt = testHashedPassword.Substring(0, 24);
            string hashedPass = testHashedPassword.Substring(24, 32);

            string inputAttempt = "ndwajfdoa12@!"; // we enter an incorrect password

            // hash the input using saved salt and check result with saved data
            string hashedInput = HashingUtility.GenerateHash(inputAttempt, salt);

            // assert that when we hash the input, it correctly fails, as the password is not correct
            Assert.AreNotEqual(hashedInput, hashedPass);
        }


        [TestMethod]
        public void SlightlyInorrectPasswordLoginsFail() // variations on case, edge cases, typos etc
        {
            string testHashedPassword = testSalt + HashingUtility.GenerateHash(testPassword, testSalt); // create a hashed password in the same way that Create Account does

            // break down saved password in same way Login Page does 
            string salt = testHashedPassword.Substring(0, 24);
            string hashedPass = testHashedPassword.Substring(24, 32);


            // try multiple:
            string inputAttempt, hashedInput;



            // we enter the password almost correct, but missed a character
            inputAttempt = "PassWord13!";

            // hash the input using saved salt and check result with saved data
            hashedInput = HashingUtility.GenerateHash(inputAttempt, salt);

            // assert that when we hash the input, it correctly fails, as the password is not correct
            Assert.AreNotEqual(hashedInput, hashedPass);



            // we enter the password almost correct, but all lowercase
            inputAttempt = "password1231"; 
            hashedInput = HashingUtility.GenerateHash(inputAttempt, salt);
            Assert.AreNotEqual(hashedInput, hashedPass);



            // we enter the password almost correct, but all uppercase
            inputAttempt = "PASSWORD123!";
            hashedInput = HashingUtility.GenerateHash(inputAttempt, salt);
            Assert.AreNotEqual(hashedInput, hashedPass);



            // we enter the password almost correct, but space at the end
            inputAttempt = "PassWord123! ";
            hashedInput = HashingUtility.GenerateHash(inputAttempt, salt);
            Assert.AreNotEqual(hashedInput, hashedPass);
        }

    }
}

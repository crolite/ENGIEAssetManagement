using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace ENGIEAssetManagementTests
{
    [TestClass]
    public class CreateAccountPageTests
    {
        // tests for the validation of usernames. whether a user can create account not tested here. firebase db methods in FirebaseConnectorTests
        // same validation logic in CreateAccountPage but simplified into methods here
        private static bool validUsername(string input)
        {
            // Username must be between 5 and 15 characters, and can contain: lower/uppercase letters, digits 0-9, and symbols '@, #, $, !, %, ^, &, +, ='
            return Regex.IsMatch(input, "^[a-zA-Z0-9@#$!%^&+=]{5,12}$");
        }

        private static bool validPass(string input)
        {
            // Password must be between 8 and 20 characters, lower+uppercase letters, digits 0-9, symbols '@, #, $, !, %, ^, &, +, =', and atleast one of each
            return Regex.IsMatch(input, "^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[@#$!%^&+=])[a-zA-Z0-9@#$!%^&+=]{8,20}$");
        }

        [TestMethod]
        public void ValidUsernamesPass()
        {
            Assert.IsTrue(Array.TrueForAll(new String[]{
                "TestUser",
                "TestUser123",
                "TestUser1&^!",
                "$!%^&+=",
                "testuser",
                "12345678",
                "TestU", // 5 char min
                "TestUser1234" // 12 char limit 
                }, validUsername));

        }

        [TestMethod]
        public void InvalidUsernamesFail()
        {
            Assert.IsFalse(validUsername("Test")); // 4 chars, 5 is min 
            Assert.IsFalse(validUsername("TestUser12345")); // 13 chars, 12 is max
            Assert.IsFalse(validUsername("TestUser*")); // disallowed character 
            // Assert.IsFalse(validUsername(null)); // this actually works but covered in code
        }

        [TestMethod]
        public void ValidPasswordsPass()
        {
            Assert.IsTrue(Array.TrueForAll(new String[]{
                "TestingPass123!",
                "I!FBIBU7bkbk@#",
                "oL!8AbDg%",
                "8BPZ0BN356$wj67Zo8#^",
                }, validPass));
        }

        [TestMethod]
        public void InvalidPasswordsFail()
        {
            Assert.IsFalse(validPass("abcdefg")); 
            Assert.IsFalse(validPass("Password"));
            Assert.IsFalse(validPass("PASS"));
            Assert.IsFalse(validPass("1234")); 
            Assert.IsFalse(validPass("oL!8bDg")); // one char too short
            Assert.IsFalse(validPass("8BPZ0BN356$wj67Zo8#^b")); // one char too long
            Assert.IsFalse(validPass("77D0Mqj5R7z506hme5AgBo&^?cmrPfbQs8NZUW0C5Rc4uwQfaB6X0cRTYP$SEC5bWFj$YL6iCj567s#nRG3p7cbhv$A8Qfmlkr!9")); // too long
            Assert.IsFalse(validPass("TestingPass123")); // no special char
            Assert.IsFalse(validPass("Test]47")); // incorrect char
            Assert.IsFalse(validPass("f£ej4aDW")); // incorrect char
        }
      
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ENGIEAssetManagement.Models
{
    public class User
    {
        public string Username { get; set; } // username is unique - while it can be set here, cannot be changed by the user in normal operation.
        public string HashedPassword { get; set; } // stored in hashed form of salt + hashed pass
        public bool Admin { get; set; } = false; // false by default. change value via xamarin database to, make sure no quotes in value. set remains as needed by GetAllUsers method
        
    }
}

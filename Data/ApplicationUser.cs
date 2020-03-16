using Microsoft.AspNetCore.Identity;
using System;

namespace JRPC_HMS.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string ProfilePicture { get; set; }
        public string ReadWhatsNew { get; set; }
    }
}

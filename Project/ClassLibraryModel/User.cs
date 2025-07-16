using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public int isProfileComplete { get; set; }
    }

    public static class UserSession
    {
        public static User CurrentUser { get; set; }
    }
}

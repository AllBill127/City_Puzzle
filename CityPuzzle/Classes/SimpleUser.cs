using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class SimpleUser
    {
        public string HashedPass { get; set; }
        public string UserName { get; set; }

        public SimpleUser() { }
        public SimpleUser(string hashedPass, string userName)
        {
            HashedPass = hashedPass;
            UserName = userName;
        }
        public SimpleUser(User user)
        {
            HashedPass = user.Pass;
            UserName = user.UserName;
        }
    }
}

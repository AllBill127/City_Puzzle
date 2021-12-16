using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;

namespace CityPuzzle.Classes
{
    public class UserVerifier : IUserVerifier
    {
        public bool CPass(string name, string pass)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(pass))
            {
                return false;
            }

            var info = Sql.ReadUsers();
            App.CurrentUser = info.SingleOrDefault(x => x.UserName.ToLower().Equals(name.ToLower()) && PassVer(pass, x.Pass));
            if (App.CurrentUser != null) Sql.SaveCurrentUser(App.CurrentUser);
            return App.CurrentUser != null;
        }
        public bool CUser(string name)
        {
            var info = Sql.ReadUsers();
            var user = info.SingleOrDefault(x => x.UserName.ToLower().Equals(name.ToLower()));
            return user == null;
        }

        public string PToH(string pass)
        {
            string passwordHash = BCryptNet.HashPassword(pass);
            return passwordHash;
        }

        public bool PassVer(string pass, string passwordHash)
        {
            bool verified = BCryptNet.Verify(pass, passwordHash);
            return verified;
        }

        public bool CheckHashPass(string name, string pass)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(pass))
            {
                return false;
            }
            var info = Sql.ReadUsers();
            App.CurrentUser = info.SingleOrDefault(x => x.UserName.ToLower().Equals(name.ToLower()) && pass.Equals(x.Pass));
            return App.CurrentUser != null;
        }
    }
}

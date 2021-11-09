using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CityPuzzle.Classes
{
    public class User :IEquatable<User>
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string Email { get; set; }
        public List<Lazy<Puzzle>> QuestsCompleted { get; set; }
        public double MaxQuestDistance { get; set; }

        IUserVerifier _verifier;

        public User(IUserVerifier ver) 
        {
            _verifier = ver;
        }
        
        public bool CheckPassword(string name, string pass)
        {
            return _verifier.CPass(name, pass);
        }

        public bool CheckUser(string name)
        {
            return _verifier.CUser(name);
        }
        public string PassToHash(string pass)
        {
            return _verifier.PToH(pass);
        }
        public bool PassVerification(string pass, string passwordHash)
        {
            return _verifier.PassVer(pass, passwordHash);
        }

        public bool Equals(User other)
        {
            if (this == null || other == null)
                return false;
            else if (other.QuestsCompleted.Count == this.QuestsCompleted.Count)
                return true;
            else
                return false;
        }
    }
}

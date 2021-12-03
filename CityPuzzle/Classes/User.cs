using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

using BCryptNet = BCrypt.Net.BCrypt;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CityPuzzle.Classes
{
    public class User :IEquatable<User>
    {
        [Key]
        [DataMember]
        [JsonProperty(PropertyName = "Id")]
        public int ID { get; set; }
        [DataMember]
        [JsonProperty(PropertyName = "FirstName")]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Pass { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int MaxQuestDistance { get; set; }
        [IgnoreDataMember]
        public List<Lazy<Puzzle>> QuestsCompleted = new List<Lazy<Puzzle>>();
        [IgnoreDataMember]
        public List<CompletedTask> CompletedTasks = new List<CompletedTask>();

        public User() { }
        public User(string userName,string pass) {
            UserName = userName;
            Pass = pass;}
        public static Boolean CheckPassword(string name, string pass)
        {
             if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(pass))
            {
                return false;
            }
            // pass = PassToHash(pass);//     //                              //encrypt
            var info = Sql.ReadUsers();
            App.CurrentUser = info.SingleOrDefault(x => x.UserName.ToLower().Equals(name.ToLower()) && PassVerification(pass,x.Pass));
            if (App.CurrentUser != null) Sql.SaveCurrentUser(App.CurrentUser);
            return App.CurrentUser != null;
        }
        public static Boolean CheckHachedPassword(string name, string pass)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(pass))
            {
                return false;
            }
            var info = Sql.ReadUsers();
            App.CurrentUser = info.SingleOrDefault(x => x.UserName.ToLower().Equals(name.ToLower()) && pass.Equals(x.Pass));
            return App.CurrentUser != null;
        }
        public static Boolean CheckUser(string name)
        {
            var info = Sql.ReadUsers();
            var user= info.SingleOrDefault(x => x.UserName.ToLower().Equals(name.ToLower()));
            return user == null;
        }

        public static string PassToHash(string pass)
        {
            string passwordHash = BCryptNet.HashPassword(pass);
            return passwordHash;
        }

        public static bool PassVerification(string pass, string passwordHash)
        {
            bool verified = BCryptNet.Verify(pass, passwordHash);
            return verified;
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
        public void Delete()
        {
            string adress = "Users/" + this.ID;

            try
            {
                App.WebServices.DeleteObject(adress);
                Console.WriteLine("Delete is working");
            }
            catch (APIFailedDeleteException ex)
            {
                Console.WriteLine("APIFailedDeleteException Error" + ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: else " + ex);
            }
        }

        public async void Save()
        {
            try
            {
                var response = await App.WebServices.SaveObject(this);
                ID = response.ID;
                Console.WriteLine("Saving is working");
            }
            catch (APIFailedSaveException ex) //reikia pagalvot kaip handlinti(galima mesti toliau ir try kur skaitoma(throw)) 
            {
                Console.WriteLine("APIFailedSaveException Error" + ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: else " + ex);
            }
        }

    }
}

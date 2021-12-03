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

        private readonly IUserVerifier _verifier;

        public User(IUserVerifier ver) 
        {
            _verifier = ver;
        }
        
        public User(string userName,string pass) 
        {
            UserName = userName;
            Pass = pass;
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

        public bool CheckHachedPassword(string name, string pass)
        {
            return _verifier.CheckHashPass(name, pass);
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

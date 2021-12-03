using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CityPuzzle.Classes
{
    [DataContract]
    public class Room
    {
        [Key][DataMember]
        [JsonProperty(PropertyName = "Id")]
        public int ID { get; set; }
        [DataMember]
        public string RoomPin { get; set; }
        [DataMember]
        public int? Owner { get; set; }
        [DataMember]
        public int? RoomSize { get; set; }

        [IgnoreDataMember]
        public List<Lazy<Puzzle>> Tasks { get; set; }
        [IgnoreDataMember]
        public List<int> ParticipantIDs{ get; set; }
        [IgnoreDataMember]
        public List<RoomTask> RoomTasks { get; set; }
        [IgnoreDataMember]
        public List<Participant> Participants { get; set; }

        public Room()
        {
            Tasks = new List<Lazy<Puzzle>>();
            ParticipantIDs = new List<int>();
        }
        public void setParticipants(User user)
        {
            ParticipantIDs.Add(user.ID);
        }

        public void SetTask(Puzzle puzzle)
        {
            Tasks.Add(new Lazy<Puzzle>(() =>puzzle));
        }

        public void Delete()
        {
            string adress = "Rooms/" + this.ID;

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
            catch (APIFailedSaveException ex)  
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

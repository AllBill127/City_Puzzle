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
    public class Room : CityPuzzleObjects
    {
        [Key]
        [DataMember]
        [JsonProperty(PropertyName = "Id")]
        public int ID { get; set; }
        [DataMember]
        public string RoomPin { get; set; }
        [DataMember]
        public int? Owner { get; set; }
        [DataMember]
        public int? RoomSize { get; set; }

        [IgnoreDataMember]
        public List<RoomTask> RoomTasks { get; set; }
        [IgnoreDataMember]
        public List<Participant> Participants { get; set; }

        public Room()
        {
            RoomTasks = new List<RoomTask>();
            Participants = new List<Participant>();
        }
        public void setParticipants(User user)
        {
            Participants.Add(new Participant()
            {
                UserId = user.ID,
                RoomId = ID
            });
        }

        public void SetTask(Puzzle puzzle)
        {
            RoomTasks.Add(new RoomTask()
            {
                PuzzleId = puzzle.ID,
                RoomId = ID
            }
            );
        }

        public void Delete()
        {
            string adress = "Rooms/" + this.ID;

            try
            {
                ApiCommands.DeleteObject(adress);
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
        public async void Save(List<int> puzzleIds)
        {
            try
            {
                var response = await ApiCommands.SaveObject(this);
                ID = response.ID;

                foreach(int puzzleId in puzzleIds)
                {
                    RoomTask rt = new RoomTask()
                    {
                        PuzzleId = puzzleId,
                        RoomId = 0,
                    };
                    rt.Save();
                    this.RoomTasks.Add(rt);
                }
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

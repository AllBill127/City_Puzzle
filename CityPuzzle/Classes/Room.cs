using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class Room
    {
        public String ID { get; set; }
        [TextBlob("addressesBlobbed")]
        public List<Puzzle> Tasks { get; set; }
        public string addressesBlobbed { get; set; }
        // [TextBlob ("addressesBlobbed")]
        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        [TextBlob("addressesBlobbed2")]
        public List<User> Participants{ get; set; }
        public string addressesBlobbed2 { get; set; }

        public Room(String roomid)
        {
            ID= roomid;
            Tasks = new List<Puzzle>();
            Participants = new List<User>();
        }
        public void setParticipants(User user)
        {
            Participants.Add(user);
        }

        public void SetTask(Puzzle puzzle)
        {
            Tasks.Add(puzzle);
        }

}
}

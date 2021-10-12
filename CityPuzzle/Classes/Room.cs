using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class Room
    {
        [PrimaryKey]
        public int Number { get; set; }
        public String ID { get; set; }
        public List<Puzzle> Tasks { get; set; }
        [TextBlob ("addressesBlobbed")]
        public List<User> Participants { get; set; }
        public string addressesBlobbed { get; set; }

        public Room(String roomid)
        {
            ID= roomid;
            Tasks = new List<Puzzle>();
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

using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class Room
    {
        public string ID { get; set; }
        public int Owner { get; set; }
        public int RoomSize { get; set; }
        public List<Lazy<Puzzle>> Tasks { get; set; }
        public List<int> ParticipantIDs{ get; set; }
       
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
        }}
}

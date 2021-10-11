using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class Room
    {
        public String ID;
        public List<Puzzle> Tasks;


        public Room(String roomid)
        {
            ID= roomid;
            Tasks = new List<Puzzle>();
        }

        public void SetTask(Puzzle puzzle)
        {
            Tasks.Add(puzzle);
        }

}
}

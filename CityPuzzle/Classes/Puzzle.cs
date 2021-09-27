using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    class Puzzle
    {
        public static int Quantity=0;
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public String About { get; set; }
        public String Quest { get; set; }
        public String Name { get; set; }
        public String ImgAdress { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Puzzle()
        {
            Quantity += 1;
        }

    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class Puzzle
    {
        public static int Quantity=0;
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string About { get; set; }
        public string Quest { get; set; }
        public string Name { get; set; }
        public string ImgAdress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Puzzle()
        {
            Quantity += 1;
        }

    }
}

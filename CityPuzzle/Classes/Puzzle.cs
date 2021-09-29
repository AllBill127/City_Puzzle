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
        public string About { get; set; }
        public string Quest { get; set; }
        public string Name { get; set; }
        public string ImgAdress { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Puzzle()
        {
            Quantity += 1;
        }

    }
}

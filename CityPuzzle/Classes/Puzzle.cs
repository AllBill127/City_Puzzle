using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CityPuzzle.Classes
{
    public class Puzzle
    {
        public static int Quantity=0;
        [PrimaryKey, AutoIncrement]
        [JsonProperty("puzzleID")]
        public int ID { get; set; }
        [JsonProperty("about")]
        public string About { get; set; }
        [JsonProperty("quest")]
        public string Quest { get; set; }
        [JsonProperty("puzzleName")]
        public string Name { get; set; }
        [JsonProperty("imageAddress")]
        public string ImgAdress { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        public Puzzle()
        {
            Quantity += 1;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

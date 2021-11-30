using System;
using System.Collections.Generic;

#nullable disable

namespace CityPuzzleWebSer.Models
{
    public partial class Puzzle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string Quest { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ImgAdress { get; set; }
    }
}

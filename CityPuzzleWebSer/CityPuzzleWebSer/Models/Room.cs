using System;
using System.Collections.Generic;

#nullable disable

namespace CityPuzzleWebSer.Models
{
    public partial class Room
    {
        public int Id { get; set; }
        public string RoomPin { get; set; }
        public int? Owner { get; set; }
        public int? RoomSize { get; set; }
        public string Tasks { get; set; }
    }
}

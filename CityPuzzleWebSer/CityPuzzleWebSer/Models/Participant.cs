using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CityPuzzleWebSer.Models
{
    public partial class Participant
    {
        
        public string RoomPin { get; set; }
        public int UserId { get; set; }
        public string TasksComplited { get; set; }
    }
}

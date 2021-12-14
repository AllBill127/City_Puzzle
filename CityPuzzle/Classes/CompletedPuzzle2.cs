using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CityPuzzle.Classes
{
    [DataContract]
    public class CompletedPuzzle2
    {
        //[Key] [DataMember]
        //public CompletedPuzzleId;
        [DataMember]
        public int UserId;
        [DataMember]
        public int PuzzleId;
        [DataMember]
        public int Score;

        public CompletedPuzzle2()
        {

        }

        public CompletedPuzzle2(int userId, int puzzleId, int score)
        {
            UserId = userId;
            PuzzleId = puzzleId;
            Score = score;
        }
    }
}

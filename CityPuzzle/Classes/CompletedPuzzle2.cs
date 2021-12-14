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
        [DataMember]
        public int CompletedPuzzleId { get; set; }
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

        public async void Save()
        {
            try
            {
                var response = await App.WebServices.SaveObject(this);
                Console.WriteLine("Saving is working");
            }
            catch (APIFailedSaveException ex) //reikia pagalvot kaip handlinti(galima mesti toliau ir try kur skaitoma(throw)) 
            {
                Console.WriteLine("APIFailedSaveException Error" + ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: else " + ex);
            }
        }

    }
}

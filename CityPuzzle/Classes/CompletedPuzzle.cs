using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CityPuzzle.Classes
{
    [DataContract]
    public partial class CompletedPuzzle : CityPuzzleObjects
    {
        [Key]
        [DataMember]
        public int UserId { get; set; }
        [Key]
        [DataMember]
        public int PuzzleId { get; set; }

        public CompletedPuzzle()
        {

        }
        public async void Save()
        {
            try
            {
                var response = await ApiCommands.SaveObject(this);
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
        public void Delete()
        {
            string json = Rest_Services.Client.APICommands.Serialize(this);
            string adress = "Tasks/"+json;

            try
            {
                App.WebServices.DeleteObject(adress);
            }
            catch (APIFailedDeleteException ex)
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

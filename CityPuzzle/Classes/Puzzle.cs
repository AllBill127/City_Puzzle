using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CityPuzzle.Classes
{
    public class Puzzle: CityPuzzleObjects
    {
        [Key]
        [DataMember]
        [JsonProperty(PropertyName = "Id")]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string About { get; set; }
        [DataMember]
        public string Quest { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public string ImgAdress { get; set; }

        public void Delete()
        {
            string adress = "Puzzles/" + this.ID;
            
            try
            {
                ApiCommands.DeleteObject(adress);

                Console.WriteLine("Delete is working");
            }
            catch (APIFailedDeleteException ex)
            {
                Console.WriteLine("APIFailedDeleteException Error" + ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: else " + ex);
            }
        }

        public async void Save()
        {
            try
            {
                var response = await ApiCommands.SaveObject(this);

                ID = response.ID;
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

        public override string ToString()
        {
            return Name;
        }
    }
}

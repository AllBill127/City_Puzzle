using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CityPuzzle.Rest_Services.Client
{
    [DataContract]
    public class ConnString: CityPuzzleObjects
    {
        [DataMember]
        public string Conn { get; set; }
        [DataMember]
        public string Token { get; set; }

        public async void ChangeConn()
        {
            try
            {
                var response = await ApiCommands.SaveObject(this);
            }
            catch (APIFailedSaveException ex)
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

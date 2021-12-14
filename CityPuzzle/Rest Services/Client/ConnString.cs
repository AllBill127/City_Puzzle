using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CityPuzzle.Rest_Services.Client
{
    [DataContract]
    public class ConnString
    {
        [DataMember]
        public string Conn { get; set; }
        [DataMember]
        public string Token { get; set; }

        public async void ChangeConn()
        {
            try
            {
                var response = await App.WebServices.SaveObject(this); //po mergo su test pakeisti
                Console.WriteLine("ChangeConn is working");
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

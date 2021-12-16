using CityPuzzle.Rest_Services.Client;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CityPuzzle.Classes
{
    public class CityPuzzleObjects
    {
        [IgnoreDataMember]
        protected static APICommands ApiCommands;

        public CityPuzzleObjects()
        { 
            ApiCommands = App.WebServices; 
        }

        public void ChangeService(APICommands api)
        {
            ApiCommands = api;
        }
    }
}

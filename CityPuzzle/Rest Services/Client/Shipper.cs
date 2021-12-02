using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CityPuzzle.Rest_Services.Client
{
    class Shipper: HttpClientRequest
    {
        public async void SaveObject<T>(T item)
        {
            string jsonItem = Serialize(item);
            SendItem("Participants", jsonItem);

        }
        public static string Serialize<T>(T item)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            string output = string.Empty;
            using (var ms = new MemoryStream())
            {
                js.WriteObject(ms, item);
                output = Encoding.Unicode.GetString(ms.ToArray());
            }
            return output;
        }
    }
}

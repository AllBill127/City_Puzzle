using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CityPuzzle.Classes;

namespace WebService
{
    public class RestService
    {
        HttpClient _client;
        public RestService()
        {
            _client = new HttpClient();
        }
        public async Task<List<Puzzle>> GetRepositoriesAsync(string uri)
        {
            List<Puzzle> repositories = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    repositories = JsonConvert.DeserializeObject<List<Puzzle>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return repositories;
        }
    }
}

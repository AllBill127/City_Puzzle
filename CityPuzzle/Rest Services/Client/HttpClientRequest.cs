using CityPuzzle.Classes;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CityPuzzle.Rest_Services.Client
{
    public class HttpClientRequest
    {
        private string url = "http://10.0.2.2:5000/api/";
        static HttpClient httpClient = new HttpClient();

        protected void SetUrl(string url)
        {
            this.url = url;
        }
        public async Task<string> SendCommand(string objectPath)
        {
            Console.WriteLine("url + objectPath" + url + objectPath);
            Task<string> sendcommand = httpClient.GetStringAsync(url + objectPath);
            Thread timer = new Thread(new ThreadStart(() => Thread.Sleep(3000)));
            timer.Start();
            while (timer.IsAlive)
            {
                if (sendcommand.IsCompleted)
                {
                    return sendcommand.Result;
                }
            }
            Console.WriteLine("Canceled");
            throw new APIFailedGetException("AFTER 3 SEC NO RESPONSE");
        }
        protected async Task<HttpResponseMessage> SendItem(string objectPath, string json)
        {

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");           
            var result = await httpClient.PostAsync(url + objectPath, content).ConfigureAwait(true);
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Complited" + result);
                var tokenJson = await result.Content.ReadAsStringAsync();
            }
            else
                throw new APIFailedSaveException("Status Code is bad");             
            return result;
        }
        protected async Task<HttpResponseMessage> DeleateItem(string objectPath)
        {
            var content = await httpClient.DeleteAsync(url + objectPath);
            return content;
        }

        public static string GetAdress<T>(T item)
        {
            Type typeParameterType = typeof(T);

            if (typeParameterType == typeof(Participant))
                return "Participants";
            if (typeParameterType == typeof(Room))
                return "Rooms";
            if (typeParameterType == typeof(Puzzle))
                return "Puzzles";
            if (typeParameterType == typeof(RoomTask))
                return "RoomTasks";
            if (typeParameterType == typeof(User))
                return "Users";
            if (typeParameterType == typeof(CompletedTask))
                return "Tasks";
            throw new Classes.TypeNotExistException();
        }
    }
}

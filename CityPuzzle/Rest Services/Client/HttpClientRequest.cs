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
        private static string defaultUrl = "http://10.0.2.2:5000/api/";
        static HttpClient httpClient = new HttpClient();

        protected void SetUrl(string url)
        {
            HttpClientRequest.defaultUrl = url;
        }
        public async Task<string> SendCommand(string objectPath)
        {
            Task<string> sendcommand = httpClient.GetStringAsync(defaultUrl + objectPath);
            Thread timer = new Thread(new ThreadStart(() => Thread.Sleep(3000)));
            timer.Start();
            //Console.WriteLine("SendCommand with:\n    DB URL: " + defaultUrl + "\n    Object: " + objectPath);
            while (timer.IsAlive)
            {
                if (sendcommand.IsCompleted)
                {
                    timer.Abort();
                    return sendcommand.Result;
                }
            }
            Console.WriteLine("SendCommands Canceled after 3s");
            throw new APIFailedGetException("No response from data base after 3s");
        }
        protected async Task<HttpResponseMessage> SendItem(string objectPath, string json)
        {
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");           
            var result = await httpClient.PostAsync(defaultUrl + objectPath, content);
            Console.WriteLine("\nSendItem to "+ defaultUrl + objectPath);
            if (result.IsSuccessStatusCode)
            {
                var tokenJson = await result.Content.ReadAsStringAsync();
                Console.WriteLine("\nObject saved to " + objectPath + " table in data base " + defaultUrl);
            }
            else
                throw new APIFailedSaveException("Saving object to data base failed");             
            return result;
        }
        protected async Task<HttpResponseMessage> DeleateItem(string objectPath)
        {
            var content = await httpClient.DeleteAsync(defaultUrl + objectPath);
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
            if (typeParameterType == typeof(CompletedPuzzle))
                return "Tasks";
            if (typeParameterType == typeof(CompletedPuzzle2))
                return "CompletedPuzzles";
            if (typeParameterType == typeof(ConnString))
                return "ChangeConectionString";
            throw new Classes.TypeNotExistException();
        }
    }
}

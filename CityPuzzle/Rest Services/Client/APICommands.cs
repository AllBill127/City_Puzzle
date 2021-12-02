using CityPuzzle.Classes;
using CityPuzzle.Rest_Services.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CityPuzzle.Rest_Services.Client
{
    public class APICommands: HttpClientRequest
    {
        private static List<Participant> participants;
        private static List<CompletedTask> tasks;
        private static List<User> users;
        private static List<Room> rooms;
        private static List<Puzzle> puzzles;
        private static List<RoomTask> roomTasks;


        public async Task<List<Participant>> GetParticipants()
        {
            var json=await SendCommand("Participants");
            var _participant = JsonConvert.DeserializeObject<List<Participant>>(json);
            participants = new List<Participant>(_participant);
            foreach (var a in participants)
            {
                Console.WriteLine(a.RoomId);
            }
            return participants;
        }
        public async Task<List<CompletedTask>> GetTasks()
        {
            var json = await SendCommand("Tasks");
            var _tasks = JsonConvert.DeserializeObject<List<CompletedTask>>(json);
            tasks = new List<CompletedTask>(_tasks);
            foreach (var a in tasks)
            {
                Console.WriteLine(a.UserId+" "+a.PuzzleId);
            }
            return tasks;
        }
        public async Task<List<User>> GetUsers()
        {
            var json = await SendCommand("Users");
            var _users = JsonConvert.DeserializeObject<List<User>>(json);
            users = new List<User>(_users);
            foreach (var a in users)
            {
                Console.WriteLine(a.ID+a.UserName);
            }
            return users;
        }
        public async Task<List<Room>> GetRooms()
        {
            var json = await SendCommand("Rooms");
            var _rooms = JsonConvert.DeserializeObject<List<Room>>(json);
            rooms = new List<Room>(_rooms);
            foreach (var a in rooms)
            {
                Console.WriteLine(a.Id + a.Owner);
            }
            return rooms;
        }
        public async Task<List<Puzzle>> GetPuzzles()
        {
            var json = await SendCommand("Puzzles");
            Console.WriteLine("json " + json.ToString());
            var _puzzles = JsonConvert.DeserializeObject<List<Puzzle>>(json);
            puzzles = new List<Puzzle>(_puzzles);
            foreach (var a in puzzles)
            {
                Console.WriteLine("iNFO "+a.Id + a.About);
            }
            Console.WriteLine("Gaunu");

            return puzzles;
        }
        public async Task<List<RoomTask>> GetRoomTasks()
        {
            var json = await SendCommand("RoomTasks");
            var _roomTasks = JsonConvert.DeserializeObject<List<RoomTask>>(json);
            roomTasks = new List<RoomTask>(_roomTasks);
            foreach (var a in puzzles)
            {
                Console.WriteLine(a.Id + a.About);
            }
            return roomTasks;
        }
        public async Task<T> SaveObject<T>(T item)//pabaigtas
        {
            Type typeParameterType = typeof(T);
            string jsonItem = Serialize(item);
            try
            {
                var response=await SendItem(GetAdress(item), jsonItem);
                Console.WriteLine("res: "+await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode)
                {
                   
                    var responseItem = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    return responseItem;
                }
                else
                {
                    throw new APIFailedSaveException("response StatusCode is bad "+ jsonItem);
                }
            }
            catch (APIFailedSaveException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new APIFailedSaveException("Unknown error");
        }
        public static string Serialize<T>(T item)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            string serializedObject = JsonConvert.SerializeObject(item);
            //string output = string.Empty;
            //using (var ms = new MemoryStream())
            //{
            //    js.WriteObject(ms, item);
            //    output = Encoding.Unicode.GetString(ms.ToArray());
            //}
            return serializedObject;
        }
        public async void DeleteObject(String adress)
        {
            var respnse = await DeleateItem(adress);
            if (respnse.IsSuccessStatusCode)
            {
                Console.WriteLine("Object Deleted");
            }
            else
                throw new APIFailedDeleteException();
        }

    }
}

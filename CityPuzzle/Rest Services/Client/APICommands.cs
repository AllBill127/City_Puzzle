using CityPuzzle.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace CityPuzzle.Rest_Services.Client
{
    public class APICommands : HttpClientRequest
    {
        private static List<Participant> participants;
        private static List<CompletedPuzzle> tasks;
        private static List<User> users;
        private static List<Room> rooms;
        private static List<Puzzle> puzzles;
        private static List<RoomTask> roomTasks;

        //Komandos su Participant
        public async Task<List<Participant>> GetParticipants()
        {
            try
            {
                var json = await SendCommand("Participants");
                var _participant = JsonConvert.DeserializeObject<List<Participant>>(json);
                participants = new List<Participant>(_participant);
                return participants;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException();
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
        }

        public async Task<List<Participant>> GetRoomParticipants(int roomid)
        {
            try
            {
                var json = await SendCommand("Participants/" + roomid);
                var _participants = JsonConvert.DeserializeObject<List<Participant>>(json);
                participants = new List<Participant>(_participants);
                return participants;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message); ;
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
        }
        //Komandos su CompletedPuzzle
        public async Task<List<CompletedPuzzle>> GetTasks()
        {
            try
            {
                var json = await SendCommand("Tasks");
                var _tasks = JsonConvert.DeserializeObject<List<CompletedPuzzle>>(json);
                tasks = new List<CompletedPuzzle>(_tasks);
                return tasks;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
        }
        public async Task<List<CompletedPuzzle>> GetUserComplitedTasks(int userId)
        {
            try
            {
                var json = await SendCommand("Tasks/" + userId);
                var _tasks = JsonConvert.DeserializeObject<List<CompletedPuzzle>>(json);
                var tasks = new List<CompletedPuzzle>(_tasks);
                return tasks;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
        }
        public async Task<List<CompletedPuzzle2>> CompletedPuzzle2(int userId)
        {
            try
            {
                var json = await SendCommand("CompletedPuzzles/" + userId);
                var _tasks = JsonConvert.DeserializeObject<List<CompletedPuzzle2>>(json);
                var tasks = new List<CompletedPuzzle2>(_tasks);
                return tasks;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
        }
        //Komandos su User
        public async Task<List<User>> GetUsers()
        {
            try
            {
                var json = await SendCommand("Users");
                var _users = JsonConvert.DeserializeObject<List<User>>(json);
                users = new List<User>(_users);
                foreach (var a in users)
                {
                    try
                    {
                        a.CompletedPuzzles = await CompletedPuzzle2(a.ID);
                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        a.CompletedPuzzles = new List<CompletedPuzzle2>();
                    }
                    catch (APIFailedGetException ex)
                    {
                        a.CompletedPuzzles = new List<CompletedPuzzle2>();
                    }
                }
                return users;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
        }

        public async Task<User> GetUser(int UserId)
        {
            try
            {
                var json = await SendCommand("Users/" + UserId);
                var user = JsonConvert.DeserializeObject<User>(json);
                try
                {
                    user.QuestsCompleted = await GetUserComplitedTasks(UserId);
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    user.QuestsCompleted = new List<CompletedPuzzle>();
                }
                catch (APIFailedGetException ex)
                {
                    user.QuestsCompleted = new List<CompletedPuzzle>();
                }
                return user;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
        }
        //Komandos su Room
        public async Task<List<Room>> GetRooms()
        {
            var json = await SendCommand("Rooms");
            try
            {
                var _rooms = JsonConvert.DeserializeObject<List<Room>>(json);
                rooms = new List<Room>(_rooms);
                foreach (var a in rooms)
                {
                    try
                    {
                        a.Participants = await GetRoomParticipants(a.ID);
                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        a.RoomTasks = new List<RoomTask>();
                    }
                    catch (APIFailedGetException ex)
                    {
                        a.RoomTasks = new List<RoomTask>();
                    }
                    try
                    {
                        a.RoomTasks = await GetRoomsTasks(a.ID);
                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        a.RoomTasks = new List<RoomTask>();
                    }
                    catch (APIFailedGetException ex)
                    {
                        a.RoomTasks = new List<RoomTask>();
                    }
                }
                return rooms;
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
        }

        public async Task<Room> GetRoom(int roomId)
        {
            try
            {
                var json = await SendCommand("Rooms/" + roomId);
                var Room = JsonConvert.DeserializeObject<Room>(json);
                try
                {
                    Room.Participants = await GetRoomParticipants(roomId);
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Room.Participants = new List<Participant>();
                }
                catch (APIFailedGetException ex)// GALIMA PERNESTI DAUG REIKLAINGOS INFORMACIJOS REIKLAINGOS TESTINIMUI
                {
                    Room.Participants = new List<Participant>();
                }
                try
                {
                    Room.RoomTasks = await GetRoomsTasks(Room.ID);
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Room.RoomTasks = new List<RoomTask>();
                }
                catch (APIFailedGetException ex)// GALIMA PERNESTI DAUG REIKLAINGOS INFORMACIJOS REIKLAINGOS TESTINIMUI
                {
                    Room.RoomTasks = new List<RoomTask>();
                }
                return Room;
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
        }
        //Komandos su Puzzle
        public async Task<List<Puzzle>> GetPuzzles()
        {
            try
            {
                var json = await SendCommand("Puzzles");
                var _puzzles = JsonConvert.DeserializeObject<List<Puzzle>>(json);
                puzzles = new List<Puzzle>(_puzzles);
                Console.WriteLine("SKAITYTI BAIGTA");
                return puzzles;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
        }

        public async Task<Puzzle> GetPuzzle(int puzzleId)
        {
            try
            {
                var json = await SendCommand("Puzzles/" + puzzleId);
                var puzzle = JsonConvert.DeserializeObject<Puzzle>(json);
                return puzzle;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
        }
        //Komandos su RoomTasks
        public async Task<List<RoomTask>> GetRoomTasks()
        {
            try
            {
                var json = await SendCommand("RoomTasks");
                var _roomTasks = JsonConvert.DeserializeObject<List<RoomTask>>(json);
                roomTasks = new List<RoomTask>(_roomTasks);
                return roomTasks;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException();
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
        }

        public async Task<List<RoomTask>> GetRoomsTasks(int roomID)
        {
            try
            {
                var json = await SendCommand("RoomTasks/" + roomID);
                var _roomTasks = JsonConvert.DeserializeObject<List<RoomTask>>(json);
                var roomTasks = new List<RoomTask>(_roomTasks);
                return roomTasks;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedGetException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedGetException("DeserializeObject error: " + ex.Message);
            }
        }
        //Komandos su Savinti
        public async Task<T> SaveObject<T>(T item)//pabaigtas
        {
            Type typeParameterType = typeof(T);
            try
            {
                string jsonItem = Serialize(item);
                var response = await SendItem(GetAdress(item), jsonItem);
                if (response.IsSuccessStatusCode)
                {

                    var responseItem = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    return responseItem;
                }
                else
                {
                    throw new APIFailedSaveException("response StatusCode is bad " + jsonItem);
                }
            }
            catch (APIFailedSaveException ex)
            {
                throw ex;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new APIFailedSaveException(ex.Message);
            }
            catch (SerializationException ex)
            {
                throw new APIFailedSaveException("Serialize error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static string Serialize<T>(T item)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            string serializedObject = JsonConvert.SerializeObject(item);
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


        public void ChangeDbSring(string conn)
        {

        }

    }
}

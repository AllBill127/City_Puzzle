using SQLite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CityPuzzle.Classes
{
    public class Sql
    {
        public static string ConnStr = "Server = tcp:citypuzzle.database.windows.net,1433; Initial Catalog = CityPuzzle; " +
            "Persist Security Info = False; User ID = citypuzzle; Password = User123*; MultipleActiveResultSets = False; " +
            "Encrypt = True; TrustServerCertificate = False; Connection Timeout=30;";

        //public static void SaveUser(User user)//SAU ZINUTE- pakeisk kad grazintu userio id, nes kai useri sukuri- jo id nesukuri!!!
        //{
        //    using (SqlConnection conn = new SqlConnection(ConnStr))
        //    {
        //        conn.Open();
        //        var command = new SqlCommand("INSERT INTO Users (UserName,FirstName,LastName,Pass,Email,MaxQuestDistance) output inserted.ID VALUES (@UserName,@FirstName,@LastName,@Pass,@Email,@MaxQuestDistance)", conn);
        //        command.Parameters.AddWithValue("@UserName", user.UserName);
        //        command.Parameters.AddWithValue("@FirstName", user.Name);
        //        command.Parameters.AddWithValue("@LastName", user.LastName);
        //        command.Parameters.AddWithValue("@Pass", user.Pass);
        //        command.Parameters.AddWithValue("@Email", user.Email);
        //        command.Parameters.AddWithValue("@MaxQuestDistance", user.MaxQuestDistance);
        //        int id = (int)command.ExecuteScalar();
        //        user.ID = id;
        //        conn.Close();
        //    }
        //    SaveComplitedTasks(user);
        //}

        public static List<User> ReadUsers()
        {
            Task<List<User>> obTask = Task.Run(() => App.WebServices.GetUsers());
            obTask.Wait();
            return obTask.Result;
        }


        public static List<Puzzle> ReadPuzzles()
        {
            Task<List<Puzzle>> obTask = Task.Run(() => App.WebServices.GetPuzzles());
            obTask.Wait();
            return obTask.Result;
        }
        public static List<Room> ReadRooms()
        {
            Task<List<Room>> obTask = Task.Run(() => App.WebServices.GetRooms());
            obTask.Wait();
            return obTask.Result;
        }
        //public static List<User> ReadUsers()
        //{
        //    using (SqlConnection conn = new SqlConnection(ConnStr))
        //    {
        //        SqlCommand command;
        //        SqlDataReader dataReader;
        //        string sql;
        //        sql = "Select Id,UserName,FirstName,LastName,Pass,Email,MaxQuestDistance from Users";
        //        conn.Open();
        //        command = new SqlCommand(sql, conn);
        //        dataReader = command.ExecuteReader();
        //        List<User> users = new List<User>();
        //        while (dataReader.Read())
        //        {
        //            User user = new User(new UserVerifier())
        //            {
        //                ID = dataReader.GetInt32(0),
        //                UserName = dataReader.GetString(1),
        //                Name = dataReader.GetString(2),
        //                LastName = dataReader.GetString(3),
        //                Pass = dataReader.GetString(4),
        //                Email = dataReader.GetString(5),
        //                MaxQuestDistance = dataReader.GetInt32(6)
        //            };
        //            user.QuestsCompleted = ReadComplitedTasks(user);
        //            users.Add(user);
        //        }
        //        conn.Close();
        //        return users;
        //    }
        //}
        ////public static void SaveComplitedTasks(User user)
        ////{
        ////    using (SqlConnection conn = new SqlConnection(ConnStr))
        ////    {
        ////        conn.Open();
        ////        foreach (Lazy<Puzzle> p in user.QuestsCompleted)
        ////        {
        ////            var command = new SqlCommand("INSERT INTO Tasks (UserID,PuzzleID) VALUES (@UserID,@PuzzleID)", conn);
        ////            command.Parameters.AddWithValue("@UserID", user.ID);
        ////            command.Parameters.AddWithValue("@PuzzleID", p.Value.ID);
        ////            command.ExecuteNonQuery();
        ////        }
        ////        conn.Close();
        ////    }
        ////}

        public static void SaveComplitedTask(Puzzle puzzle)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Tasks (UserID,PuzzleID) VALUES (@UserID,@PuzzleID)", conn);
                command.Parameters.AddWithValue("@UserID", App.CurrentUser.ID);
                command.Parameters.AddWithValue("@PuzzleID", puzzle.ID);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        //public static List<Lazy<Puzzle>> ReadComplitedTasks(User user)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(ConnStr))
        //        {
        //            SqlCommand command;
        //            SqlDataReader dataReader;
        //            string sql;
        //            sql = "Select PuzzleID from Tasks where UserID=@UserID";
        //            conn.Open();
        //            command = new SqlCommand(sql, conn);
        //            command.Parameters.AddWithValue("@UserID", user.ID);
        //            dataReader = command.ExecuteReader();
        //            List<Lazy<Puzzle>> allPuzzles = ReadPuzzles();
        //            List<Lazy<Puzzle>> puzzles = new List<Lazy<Puzzle>>();
        //            while (dataReader.Read())
        //            {
        //                int puzzleID = dataReader.GetInt32(0);
        //                Lazy<Puzzle> complitedPuzzle = allPuzzles.SingleOrDefault(x => x.Value.ID == puzzleID);
        //                puzzles.Add(complitedPuzzle);

        //            }

        //            conn.Close();
        //            return puzzles;
        //        }
        //    }
        //    catch (System.Data.SqlClient.SqlException)
        //    {
        //        return new List<Lazy<Puzzle>>();
        //    }
        //}

        // -------------------------------------------------Puzzle--------------------------------------------------------------
        public static void SavePuzzle(Puzzle puzzle)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Puzzles (Name,About,Quest,Latitude,Longitude,ImgAdress) VALUES (@Name,@About,@Quest,@Latitude,@Longitude,@ImgAdress)", conn);
                command.Parameters.AddWithValue("@Name", puzzle.Name);
                command.Parameters.AddWithValue("@About", puzzle.About);
                command.Parameters.AddWithValue("@Quest", puzzle.Quest);
                command.Parameters.AddWithValue("@Latitude", puzzle.Latitude.ToString());
                command.Parameters.AddWithValue("@Longitude", puzzle.Longitude.ToString());
                command.Parameters.AddWithValue("@ImgAdress", puzzle.ImgAdress);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        //public static List<Lazy<Puzzle>> ReadPuzzles()
        //{
        //    using (SqlConnection conn = new SqlConnection(ConnStr))
        //    {
        //        SqlCommand command;
        //        SqlDataReader dataReader;
        //        string sql;
        //        sql = "Select ID,Name,About,Quest,Latitude,Longitude,ImgAdress from Puzzles";
        //        conn.Open();
        //        command = new SqlCommand(sql, conn);
        //        dataReader = command.ExecuteReader();
        //        List<Lazy<Puzzle>> puzzles = new List<Lazy<Puzzle>>();
        //        while (dataReader.Read())
        //        {
        //            Puzzle puz = new Puzzle()
        //            {
        //                ID = dataReader.GetInt32(0),
        //                Name = dataReader.GetString(1),
        //                About = dataReader.GetString(2),
        //                Quest = dataReader.GetString(3),
        //                Latitude = Convert.ToDouble(dataReader.GetString(4)),
        //                Longitude = Convert.ToDouble(dataReader.GetString(5)),
        //                ImgAdress = dataReader.GetString(6)
        //            };
        //            Lazy<Puzzle> puzzle = new Lazy<Puzzle>(() => puz);
        //            puzzles.Add(puzzle);
        //        }

        //        conn.Close();
        //        return puzzles;
        //    }
        //}
        // -------------------------------------------------Rooms--------------------------------------------------------------
        //public static void SaveRoom(Lazy<Room> room)
        //{
        //    var numbers = room.Value.Tasks.Select(x => x.Value.ID);
        //    string converted = string.Join("-", numbers) + "-";
        //    using (SqlConnection conn = new SqlConnection(ConnStr))
        //    {
        //        conn.Open();
        //        var command = new SqlCommand("INSERT INTO Rooms (RoomPin,Owner,RoomSize,Tasks) VALUES (@RoomPin,@Owner,@RoomSize,@Tasks)", conn);
        //        command.Parameters.AddWithValue("@RoomPin", room.Value.ID);
        //        command.Parameters.AddWithValue("@Owner", room.Value.Owner);
        //        command.Parameters.AddWithValue("@RoomSize", room.Value.RoomSize);
        //        command.Parameters.AddWithValue("@Tasks", converted);
        //        command.ExecuteNonQuery();
        //        conn.Close();
        //    }
        //}
        //public static List<Room> ReadRooms()
        //{
        //    using (SqlConnection conn = new SqlConnection(ConnStr))
        //    {
        //        SqlCommand command;
        //        SqlDataReader dataReader;
        //        string sql = "Select RoomPin,Owner,RoomSize,Tasks from Rooms";
        //        conn.Open();
        //        command = new SqlCommand(sql, conn);
        //        dataReader = command.ExecuteReader();
        //        List<Room> rooms = new List<Room>();
        //        while (dataReader.Read())
        //        {
        //            Room room = new Room()
        //            {
        //                // Id = dataReader.GetString(0),
        //                Owner = dataReader.GetInt32(1),
        //                RoomSize = dataReader.GetInt32(2),
        //                Tasks = ConvertTasks(dataReader.GetString(3))
        //            };
        //            room.ParticipantIDs = FindRoomParticipantsID(room.ID);
        //            rooms.Add(room);
        //        }
        //        conn.Close();
        //        Console.WriteLine("Grazinu rooms");
        //        return rooms;
        //    }
        //}
        public static void SaveParticipants(string roomId, int userID)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Participants (RoomPin,UserID) VALUES (@RoomPin,@UserID)", conn);
                command.Parameters.AddWithValue("@RoomPin", roomId);
                command.Parameters.AddWithValue("@UserID", userID);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static List<string> FindParticipantRoomsIDs(int userID)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command;
                SqlDataReader dataReader;
                string sql = "Select RoomPin from Participants where UserID=@UserID";
                conn.Open();
                command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@UserID", userID);
                dataReader = command.ExecuteReader();
                List<string> roomPins = new List<string>();
                while (dataReader.Read())
                {
                    roomPins.Add(dataReader.GetString(0));
                }
                conn.Close();
                return roomPins;
            }
        }
        public static List<int> FindRoomParticipantsID(int roomID)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command;
                SqlDataReader dataReader;
                string sql = "Select UserID from Participants where RoomPin=@RoomPin";
                conn.Open();
                command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@RoomPin", roomID);
                dataReader = command.ExecuteReader();
                List<int> participantsID = new List<int>();
                while (dataReader.Read())
                {
                    participantsID.Add(dataReader.GetInt32(0));
                }
                conn.Close();
                return participantsID;
            }
        }
        //private static List<Puzzle> ConvertTasks(string strtask)
        //{
        //    List<Lazy<Puzzle>> allpuzzles = new List<Lazy<Puzzle>>();
        //    allpuzzles = ReadPuzzles();
        //    List<int> TaskIDs = new List<int>();
        //    string myStr = "";
        //    foreach (char i in strtask.ToCharArray())
        //    {
        //        if (i == '-')
        //        {
        //            TaskIDs.Add(int.Parse(myStr));
        //            myStr = "";
        //        }
        //        else
        //        {
        //            myStr += i;
        //        }
        //    }
        //    List<Lazy<Puzzle>> tasks = new List<Lazy<Puzzle>>();
        //    List<Lazy<Puzzle>> puzzles = ReadPuzzles();
        //    foreach (int Id in TaskIDs)
        //    {
        //        Lazy<Puzzle> task = puzzles.SingleOrDefault(x => x.Value.ID.Equals(Id));
        //        tasks.Add(task);
        //    }
        //    return tasks;
        //}

        

        public static SimpleUser GetCurrentUser()
        {
            try
            {
                List<SimpleUser> info;
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<SimpleUser>();
                    info = conn.Table<SimpleUser>().ToList();
                }
                return null;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public static void SaveCurrentUser(User user)
        {
            SimpleUser simpleUser = new SimpleUser(user);
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                user.QuestsCompleted = null;
                conn.CreateTable<SimpleUser>();
                conn.DeleteAll<SimpleUser>();
                var rows = conn.Insert(simpleUser);
            }
        }
    }
}
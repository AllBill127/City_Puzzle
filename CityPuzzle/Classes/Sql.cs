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

        // -------------------------------------------------Rooms--------------------------------------------------------------
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
                string sql = "Select RoomPin from Rooms where Owner=@UserID";
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

        //-------------------------------------------------Local User-----------------------------------------------------------------------
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
                return info[0];
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
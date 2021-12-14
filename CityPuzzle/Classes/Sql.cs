using CityPuzzle.Rest_Services.Client;
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
        public static User ReadUser(int id)
        {
            Task<User> obTask = Task.Run(() => App.WebServices.GetUser(id));
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

        public static List<Room> ReadUserRooms()
        {
            List<Room> allRooms = ReadRooms();

            Task<List<Room>> taskFindUserRooms = Task.Run(() =>
                (from room in allRooms
                 where room.Owner == App.CurrentUser.ID
                 select room).ToList()
                 );
            taskFindUserRooms.Wait();

            return taskFindUserRooms.Result;
        }

        // -------------------------------------------------Change db--------------------------------------------------------------

        public static void ChangeDb(string conn)
        {
            ConnString connString = new ConnString() { Conn = conn, Token = "CityPuzzle" };
            connString.ChangeConn();
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
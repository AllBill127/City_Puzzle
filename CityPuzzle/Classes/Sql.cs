using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
// del laizy i gamerooma nes Usually this is preferable when the object may or may not be used and the cost of constructing it is non-trivial.




namespace CityPuzzle.Classes
{
    class Sql
    {
         public static string ConnStr = "Server=tcp:citypuzzle.database.windows.net,1433;Initial Catalog=CityPuzzle;Persist Security Info=False;User ID=citypuzzle;Password=User123*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static void SaveUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Users (UserName,FirstName,LastName,Pass,Email,MaxQuestDistance) VALUES (@UserName,@FirstName,@LastName,@Pass,@Email,@MaxQuestDistance)", conn);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@FirstName", user.Name);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Pass", user.Pass);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@MaxQuestDistance", user.MaxQuestDistance);
                command.ExecuteNonQuery();
                conn.Close();
            }
            SaveComplitedTasks(user);
        }

        public static List<User> ReadUsers()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command;
                SqlDataReader dataReader;
                String sql;
                sql = "Select ID,UserName,FirstName,LastName,Pass,Email,MaxQuestDistance from Users";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                List<User> users = new List<User>();
                while (dataReader.Read())
                {
                    User user = new User()
                    {
                        ID = dataReader.GetInt32(0),
                        UserName = dataReader.GetString(1),
                        Name = dataReader.GetString(2),
                        LastName = dataReader.GetString(3),
                        Pass = dataReader.GetString(4),
                        Email = dataReader.GetString(5),
                        MaxQuestDistance = dataReader.GetInt32(6)
                        
                };
                    user.QuestsCompleted = ReadComplitedTasks(user);
                    users.Add(user);
                }
                conn.Close();
                return users;
            }
        }

        public static void SaveComplitedTasks(User user)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                foreach (Lazy<Puzzle> p in user.QuestsCompleted)
                {
                    var command = new SqlCommand("INSERT INTO Tasks (UserID,PuzzleID) VALUES (@UserID,@PuzzleID)", conn);
                    command.Parameters.AddWithValue("@UserID", user.ID);
                    command.Parameters.AddWithValue("@PuzzleID", p.Value.ID);
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
      
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
      
        public static List<Lazy<Puzzle>> ReadComplitedTasks(User user)
        {
            //System.Data.SqlClient.SqlException
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    SqlCommand command;
                    SqlDataReader dataReader;
                    string sql;
                    sql = "Select PuzzleID from Users where UserID=@UserID";
                    conn.Open();
                    command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@UserID", user.ID);
                    dataReader = command.ExecuteReader();
                    List<Lazy<Puzzle>> allPuzzles = ReadPuzzles();
                    List<Lazy<Puzzle>> puzzles = new List<Lazy<Puzzle>>();
                    while (dataReader.Read())
                    {
                        int puzzleID = dataReader.GetInt32(0);
                        Lazy<Puzzle> complitedPuzzle = allPuzzles.SingleOrDefault(x => x.Value.ID == puzzleID);
                        puzzles.Add(complitedPuzzle);

                    }

                    conn.Close();
                    return puzzles;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                return new List<Lazy<Puzzle>>();
            }

        }
      
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
      
        public static List<Lazy<Puzzle>> ReadPuzzles()// return all lazy puzzles
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command;
                SqlDataReader dataReader;
                string sql;
                sql = "Select ID,Name,About,Quest,Latitude,Longitude,ImgAdress from Puzzles";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                List<Lazy<Puzzle>> puzzles = new List<Lazy<Puzzle>>();
                while (dataReader.Read())
                {
                    Puzzle puz=new Puzzle()
                    {
                        ID = dataReader.GetInt32(0),
                        Name = dataReader.GetString(1),
                        About = dataReader.GetString(2),
                        Quest = dataReader.GetString(3),
                        Latitude = Convert.ToDouble(dataReader.GetString(4)),
                        Longitude = Convert.ToDouble(dataReader.GetString(5)),
                        ImgAdress = dataReader.GetString(6)
                    };
                    Lazy<Puzzle> puzzle = new Lazy<Puzzle>(() => puz);
                    puzzles.Add(puzzle);
                }

                conn.Close();
                foreach (Lazy<Puzzle> a in puzzles)
                {
                    Console.WriteLine(a.Value.Name);
                }
                return puzzles;
               
            }
        }
        public static void SaveRoom(Room room)
        {
            // Func<List <Puzzle>, string> convert =
            var numbers = room.Tasks.Select(x => x.Value.ID);
            string converted=string.Join("-", numbers)+"-";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Rooms (RoomPin,Owner,RoomSize,Tasks) VALUES (@RoomPin,@Owner,@RoomSize,@Tasks)", conn);
                command.Parameters.AddWithValue("@RoomPin", room.ID);
                command.Parameters.AddWithValue("@Owner", room.Owner);
                command.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                command.Parameters.AddWithValue("@Tasks", converted);
                command.ExecuteNonQuery();/// exceptionai del System.Data.SqlClient.SqlException: 
                conn.Close();
            }
        }
        public static List<Room> ReadRooms()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command;
                SqlDataReader dataReader;
                String sql = "Select RoomPin,Owner,RoomSize,Tasks from Rooms";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                List<Room> rooms = new List<Room>();
                while (dataReader.Read())
                {
                    Room room = new Room()
                    {
                        ID = dataReader.GetString(0),
                        Owner = dataReader.GetInt32(1),
                        RoomSize = dataReader.GetInt32(2),
                        Tasks = ConvertTasks(dataReader.GetString(3)) 
                    };
                    rooms.Add(room);
                }

                conn.Close();
                return rooms;

            }
        }
                                                           
        private static List<Lazy<Puzzle>> ConvertTasks(string strtask)
        {
            List<Lazy<Puzzle>> allpuzzles = new List<Lazy<Puzzle>>();
            allpuzzles = ReadPuzzles();
            List<int> TaskIDs = new List<int>();
            string myStr = "";
            foreach (char i in strtask.ToCharArray())
            {
                if (i == '-')
                {
                    TaskIDs.Add(Int32.Parse(myStr));
                    myStr = "";
                }
                else myStr += i;
            }
            List<Lazy<Puzzle>> tasks = new List<Lazy<Puzzle>>();
            List<Lazy<Puzzle>> puzzles = ReadPuzzles();
            foreach (int Id in TaskIDs)
            {
                Lazy<Puzzle> task = puzzles.SingleOrDefault(x => x.Value.ID.Equals(Id));
                tasks.Add(task);
            }
            return tasks;
        }
                                                           
        public static Puzzle FromLazy(Lazy<Puzzle> puzzle) {
            Puzzle p = new Puzzle() {
                ID = puzzle.Value.ID,
                About=puzzle.Value.About,
                Name = puzzle.Value.Name,
                Latitude = puzzle.Value.Latitude,
                Longitude = puzzle.Value.Longitude,
                ImgAdress = puzzle.Value.ImgAdress,
                Quest = puzzle.Value.Quest};
            return p;
        }
       
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;



namespace CityPuzzle.Classes
{
    class Sql
    {
        public static string ConnStr = "Server = tcp:citypuzzledb.database.windows.net,1433;Initial Catalog = citypuzzledb; Persist Security Info=False;User ID = citypuzzleuser; Password=User123*; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

        public static void SaveUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Users (UserName,FirstName,LastName,Pass,Email) VALUES (@UserName,@FirstName,@LastName,@Pass,@Email)", conn);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@FirstName", user.Name);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Pass", user.Pass);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static String ReadUsers()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command;
                SqlDataReader dataReader;
                String sql, Output = "";

                sql = "Select ID,UserName,FirstName,LastName,Pass,Email from Users";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                List<User> users = new List<User>();
                while (dataReader.Read())
                {
                    Output = Output + dataReader.GetValue(0) + "-" + dataReader.GetValue(1) + "-" + dataReader.GetValue(3) + "-" + dataReader.GetValue(4) + "-" + dataReader.GetValue(5);
                    User user = new User()
                    {
                        ID = dataReader.GetInt32(0),
                        UserName = dataReader.GetString(1),
                        Name = dataReader.GetString(2),
                        LastName = dataReader.GetString(3),
                        Pass = dataReader.GetString(4),
                        Email = dataReader.GetString(5)

                };
                    users.Add(user);
                }

                conn.Close();
                return Output;

            }
        }

        public static void SavePuzzle(Puzzle puzzle)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Puzzles (Name,About,Quest,Latitude,Longitude,ImgAdress) VALUES (@Name,@About,@Quest,@Latitude,@Longitude,@ImgAdress)", conn);
                command.Parameters.AddWithValue("@Name", puzzle.Name);
                command.Parameters.AddWithValue("@About", puzzle.About);
                command.Parameters.AddWithValue("@Quest", puzzle.Quest);
                command.Parameters.AddWithValue("@Latitude", puzzle.Latitude);
                command.Parameters.AddWithValue("@Longitude", puzzle.Longitude);
                command.Parameters.AddWithValue("@ImgAdress", puzzle.ImgAdress);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static String ReadPuzzles()
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command;
                SqlDataReader dataReader;
                String sql, Output = "";

                sql = "Select ID,Name,About,Quest,Latitude,Longitude,ImgAdress from Puzzles";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                List<Puzzle> puzzles = new List<Puzzle>();
                while (dataReader.Read())
                {
                    Output = Output + dataReader.GetValue(0) + "-" + dataReader.GetValue(1) + "-" + dataReader.GetValue(3) + "-" + dataReader.GetValue(4) + "-" + dataReader.GetValue(5);
                    Puzzle puzzle = new Puzzle()
                    {
                        ID = dataReader.GetInt32(0),
                        Name = dataReader.GetString(1),
                        About = dataReader.GetString(2),
                        Quest = dataReader.GetString(3),
                        Latitude = dataReader.GetDouble(4),
                        Longitude = dataReader.GetDouble(5),
                        ImgAdress = dataReader.GetString(6)
                    };
                    puzzles.Add(puzzle);
                }

                conn.Close();
                return Output;

            }
        }
    }
}

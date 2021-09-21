using SQLite;
using System;

namespace CityPuzzle.Classes
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public User() { }
        public static Boolean CheckPassword(string name, string pass)
        {
             if (name == null || pass == null)
            {
                return false;
            }
            else if (name.Length == 0 || pass.Length == 0 )
            {
                return false;
            }
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<User>();

                var info = conn.Table<User>().ToList();
                foreach (User n in info)
                {
                    if (n.UserName.ToLower().Equals(name.ToLower()) && n.Pass.ToLower().Equals(pass.ToLower()))
                    {
                        App.CurrentUser = n;
                        return true;
                    }


                }
            };
            return false;
        }
        public static Boolean CheckUser(string name)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<User>();

                var info = conn.Table<User>().ToList();
                Console.WriteLine(info.Count);
                foreach (User n in info)
                {
                    if (n.UserName.ToLower().Equals(name.ToLower()))
                    {
                        return false;
                    }
                }

            };
            return true;
        }
    }



}

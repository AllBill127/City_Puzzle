using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public static class ExtensionMethods
    {
        // Extension method for User class finding top 10 users by completed quest count
        public static List<User> Top10(this List<User> users)
        {
            users.Sort(new PointsComparer());

            List<User> top10 = new List<User>();
            for (int i = 0; i < 10; ++i)
            {
                top10.Add(users[i]);
            }

            return top10;
        }
    }
}

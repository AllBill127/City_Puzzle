using System;
using System.Collections.Generic;
using System.Text;
using static CityPuzzle.Classes.Structs;

namespace CityPuzzle.Classes
{
    public static class ExtensionMethods
    {
        // Extension method for User class finding top 10 users by completed quest count
        public static List<UserInfo> Top10(this List<User> users)
        {
            users.Sort(new PointsComparer());
            users.Reverse();

            List<UserInfo> TopUsers = new List<UserInfo>();

            User prev = null;
            int j = 0; //nusako rezultato indexa 
            UserInfo temp = new UserInfo();

            foreach (var user in users)
            {
                if (user.Equals(prev) == false)
                    ++j;

                temp.Username = user.UserName;
                temp.Score = user.QuestsCompleted.Count;
                temp.Index = j;
                
                TopUsers.Add(temp);

                prev = user;
            }
            return TopUsers;
        }
    }
}

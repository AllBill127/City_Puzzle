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

            List<UserInfo> TopUsers = new List<UserInfo>();

            int i = 0; //iteruoja pro listus
            int j = 1; //nusako rezultato indexa 
            UserInfo temp = new UserInfo { };

            while (j <= 10 && users[i] != null)
            {  
                temp.name = users[i].Name;
                temp.score = users[i].QuestsCompleted.Count;
                temp.index = j;
                
                TopUsers[i] = temp;
                
                if (users[i + 1].Equals(users[i]) == false)
                    ++j;

                ++i;
            }
            return TopUsers;
        }
    }
}

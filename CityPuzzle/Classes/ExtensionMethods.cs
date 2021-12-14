using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CityPuzzle.Classes.Structs;

namespace CityPuzzle.Classes
{
    public static class ExtensionMethods
    {
        // Extension method for User class finding top 10 users by completed quest count
        public static List<UserInfo> Top10(this List<User> users)
        {
            var comparer = new PointsComparer();
            users.Sort(comparer);
            users.Reverse();

            List<UserInfo> TopUsers = new List<UserInfo>();

            User prev = null;
            int j = 0; //nusako rezultato indexa 
            UserInfo temp = new UserInfo();

            foreach (var user in users)
            {
                if (user.Equals(prev) == false)
                    ++j;
                if (j > 10) 
                    break;

                temp.Username = user.UserName;
                temp.Score = user.CompletedPuzzles.Aggregate(0, (score, next) => score += next.Score);  // user.QuestsCompleted.Count;
                temp.Index = j;

                TopUsers.Add(temp);

                prev = user;
            }
            return TopUsers;
        }

        public static List<TResult> Top10Cast<TResult, IComparer, TSource> (this List<TSource> items,
            IComparer comparer, Func<TSource, int, TResult> cast) where TSource : IEquatable<TSource>
        {
            items.Sort((IComparer<TSource>)comparer);
            items.Reverse();

            List<TResult> topItems = new List<TResult>();
            int index = 1;              // Place in the leaderboard
            TSource prev = items[0];

            foreach (var item in items)
            {
                if (item.Equals(prev) == false)
                    ++index;

                if (index > 10)        // end topList when all 10th places are added 
                    break;

                topItems.Add(cast(item, index));    // add a casted item with its place in the leaderboard to topList

                prev = item;
            }

            return topItems;
        }

        public static List<TResult> CastToLeaderboard<TResult, IComparer, TSource>(this List<TSource> items,
            IComparer comparer, Func<TSource, int, TResult> cast) where TSource : IEquatable<TSource>
        {
            items.Sort((IComparer<TSource>)comparer);
            items.Reverse();

            List<TResult> list = new List<TResult>();
            int index = 1;              // Place in the leaderboard
            TSource prev = items[0];

            foreach (var item in items)
            {
                if (item.Equals(prev) == false)
                    ++index;

                list.Add(cast(item, index));    // add a casted item with its place in the leaderboard to topList

                prev = item;
            }

            return list;
        }
    }
}
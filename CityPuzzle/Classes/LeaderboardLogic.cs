using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityPuzzle.Classes;
using static CityPuzzle.Classes.Structs;

namespace CityPuzzle.Classes
{
    public class LeaderboardLogic
    {
        private List<UserInfo> lbUsers = null;
        private int pageNr = 0;
        private IEnumerable<UserInfo> pageList = null;

        public event EventHandler<OnPageChangeEventArgs> OnPageChange;
        public class OnPageChangeEventArgs : EventArgs { public int PageNr; public IEnumerable<UserInfo> PageList; }

        public LeaderboardLogic()
        {
            // Form a sorted list of user info items for leaderboard
            lbUsers = Sql.ReadUsers().CastToLeaderboard(new PointsComparer(), (user, index) => new UserInfo
            {
                Username = user.UserName,
                Score = user.CompletedPuzzles.Aggregate(0, (score, next) => score += next.Score),   //user.QuestsCompleted.Count,
                Index = index
            });
        }

        public void ChangePage(bool direction)
        {
            if (pageNr == 0)
            {
                pageNr++;
                pageList = lbUsers.Take(10);
            }
            else if (pageNr > 1 && !direction)
            {
                pageNr--;
                pageList = lbUsers.Skip((pageNr - 1) * 10).Take(10);
            }
            else if (pageNr < lbUsers.Count / 10 && direction)
            {
                pageNr++;
                pageList = lbUsers.Skip((pageNr - 1) * 10).Take(10);
            }
            else if (pageNr == lbUsers.Count / 10 && direction)
            {
                pageNr++;
                pageList = lbUsers.Skip((pageNr - 1) * 10).Take(lbUsers.Count % 10);
            }
            else
                return;

            OnPageChange?.Invoke(this, new OnPageChangeEventArgs { PageNr = pageNr, PageList = pageList });
        }
    }
}

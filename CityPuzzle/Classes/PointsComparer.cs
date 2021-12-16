using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityPuzzle.Classes
{
    public class PointsComparer : IComparer<User>
    {
        public int Compare(User U1, User U2)
        {
            int u1Score = U1.CompletedPuzzles.Aggregate(0, (score, next) => score += next.Score);
            int u2Score = U2.CompletedPuzzles.Aggregate(0, (score, next) => score += next.Score);
            return u1Score.CompareTo(u2Score);
            //return U1.QuestsCompleted.Count.CompareTo(U2.QuestsCompleted.Count);
        }
    }
}

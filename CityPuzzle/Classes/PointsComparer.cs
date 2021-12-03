using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class PointsComparer : IComparer<User>
    {
        public int Compare(User U1, User U2)
        {
            return U1.QuestsCompleted.Count.CompareTo(U2.QuestsCompleted.Count);
        }
    }
}

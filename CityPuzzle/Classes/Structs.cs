﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class Structs
    {
        public struct UserInfo
        {
            public int Index { get; set; }
            public int Score { get; set; }
            public string Username { get; set; }
        }

        public struct PuzzleScoreInfo
        {
            public string PuzzleName { get; set; }
            public string ImgAdress { get; set; }
            public int Score { get; set; }
        }
    }
}

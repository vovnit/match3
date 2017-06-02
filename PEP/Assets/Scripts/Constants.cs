using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class Constants
    {
        public static readonly int Rows = 8;
        public static readonly int Columns = 8;
        public static readonly float AnimationDuration =  0.2f;

        public static readonly float MoveAnimationMinDuration = 0.05f;

        public static readonly float ExplosionDuration = 0.3f;
        
        public static readonly float WaitBeforePotentialMatchesCheck = 30f;
        public static readonly float OpacityAnimationFrameDelay = 0.05f;

        public static readonly int MinimumMatches = 3;
        public static readonly int MinimumMatchesForBonus = 4;

        public static readonly int Match3Score = 60;
        public static readonly int SubsequentMatchScore = 1000;

        public static int Mode = 1;
        public static int Score = 10000;
        public static int CurrentLevel = 0;
        public static int RequiredMoves=10;
        public static float RequiredTime = 45f;
        public static Vector2 AreaForBan1 = new Vector2(2, 2);
        public static Vector2 AreaForBan2 = new Vector2(4,4);
        public static Vector2 AreaForPoints1 = new Vector2(2, 2);
        public static Vector2 AreaForPoints2 = new Vector2(4, 4);

        //public static int ResultScore = 0;
        //public static int NumberOfStars = 0;

    }

   


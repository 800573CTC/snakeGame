using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// I may or may not use any of this


namespace snakeGame
{
    public static class GameSettings
    {
        public static int BoostSpeed { get; set; } = 50;
        
        public static double WallDensity { get; set; } = .01;
        // 20% of grid is wall      value is subject to change

        public static bool WallFatality { get; set; } = false;

        public static bool HardDeath { get; set; } = false;
    }
}

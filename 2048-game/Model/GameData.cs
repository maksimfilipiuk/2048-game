using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_game.Model
{
    static class GameData
    {
        /*
                [][][][]
                [][][][]
                [][][][]
                [][][][]
        */
        static int[,] mainArray = new int[4,4];

        public static int[,] MainArray { get => mainArray; set => mainArray = value; }

        static GameData()
        {
            mainArray[0, 0] = -1;
        }
    }
}

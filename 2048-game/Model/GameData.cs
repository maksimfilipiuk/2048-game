using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_game.Model
{
    class GameData
    {
        /*
                [][][][]
                [][][][]
                [][][][]
                [][][][]
        */
        private int[,] mainArray = new int[4,4];

        public int[,] MainArray { get => mainArray; set => mainArray = value; }

        #region Свойства для всех ячеек главного массива

        public string MainArray00 { get => (MainArray[0, 0] == 0) ? (" ") : (MainArray[0, 0].ToString()); }
        public string MainArray01 { get => (MainArray[0, 1] == 0) ? (" ") : (MainArray[0, 1].ToString()); }
        public string MainArray02 { get => (MainArray[0, 2] == 0) ? (" ") : (MainArray[0, 2].ToString()); }
        public string MainArray03 { get => (MainArray[0, 3] == 0) ? (" ") : (MainArray[0, 3].ToString()); }

        public string MainArray10 { get => (MainArray[1, 0] == 0) ? (" ") : (MainArray[1, 0].ToString()); }
        public string MainArray11 { get => (MainArray[1, 1] == 0) ? (" ") : (MainArray[1, 1].ToString()); }
        public string MainArray12 { get => (MainArray[1, 2] == 0) ? (" ") : (MainArray[1, 2].ToString()); }
        public string MainArray13 { get => (MainArray[1, 3] == 0) ? (" ") : (MainArray[1, 3].ToString()); }

        public string MainArray20 { get => (MainArray[2, 0] == 0) ? (" ") : (MainArray[2, 0].ToString()); }
        public string MainArray21 { get => (MainArray[2, 1] == 0) ? (" ") : (MainArray[2, 1].ToString()); }
        public string MainArray22 { get => (MainArray[2, 2] == 0) ? (" ") : (MainArray[2, 2].ToString()); }
        public string MainArray23 { get => (MainArray[2, 3] == 0) ? (" ") : (MainArray[2, 3].ToString()); }

        public string MainArray30 { get => (MainArray[3, 0] == 0) ? (" ") : (MainArray[3, 0].ToString()); }
        public string MainArray31 { get => (MainArray[3, 1] == 0) ? (" ") : (MainArray[3, 1].ToString()); }
        public string MainArray32 { get => (MainArray[3, 2] == 0) ? (" ") : (MainArray[3, 2].ToString()); }
        public string MainArray33 { get => (MainArray[3, 3] == 0) ? (" ") : (MainArray[3, 3].ToString()); }

        #endregion

        public GameData()
        {
            mainArray[0, 0] = -1;
        }

        public GameData(int[,] mainArray)
        {
            this.mainArray = mainArray;
        }
    }
}

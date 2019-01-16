using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using _2048_game.Model;
using _2048_game.Infrastructure;
using System.Windows.Input;

namespace _2048_game.ViewModel
{
    class GameWindowViewModel : ViewModelBase
    {
        public int[,] MainArray
        {
            get
            {
                if (GameData.MainArray[0, 0] == -1)
                {
                    GameData.MainArray = GenerateBeginState();
                }

                return GameData.MainArray;
            }
            set
            {
                GameData.MainArray = value;
                OnPropertyChanged("MainArray");
            }
        }

        #region Свойства команд для клавиш управления

        RelayCommand leftArrowCommand;
        public ICommand LeftArrowCommand
        {
            get
            {
                if(leftArrowCommand == null)
                {
                    leftArrowCommand = new RelayCommand(ExecuteLeftArrowCommand);
                }

                return leftArrowCommand;
            }
        }

        RelayCommand rightArrowCommand;
        public ICommand RightArrowCommand
        {
            get
            {
                if (rightArrowCommand == null)
                {
                    rightArrowCommand = new RelayCommand(ExecuteRightArrowCommand);
                }

                return rightArrowCommand;
            }
        }

        RelayCommand upArrowCommand;
        public ICommand UpArrowCommand
        {
            get
            {
                if (upArrowCommand == null)
                {
                    upArrowCommand = new RelayCommand(ExecuteUpArrowCommand);
                }

                return upArrowCommand;
            }
        }

        RelayCommand downArrowCommand;
        public ICommand DownArrowCommand
        {
            get
            {
                if (downArrowCommand == null)
                {
                    downArrowCommand = new RelayCommand(ExecuteDownArrowCommand);
                }

                return downArrowCommand;
            }
        }

        #endregion

        #region Свойства команд разработчика

        RelayCommand showArrayStateCommand;
        public ICommand ShowArrayStateCommand
        {
            get
            {
                if(showArrayStateCommand == null)
                {
                    showArrayStateCommand = new RelayCommand(ExecuteShowArrayStateCommand);
                }

                return showArrayStateCommand;
            }
        }

        #endregion

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

        private int[,] GenerateBeginState()
        {
            int[,] tempArray = new int[4, 4];

            Random random = new Random();

            int a = random.Next(4), b = random.Next(4), c = random.Next(4), d = random.Next(4);

            while (a == c && b == d)
            {
                a = random.Next(4);
                b = random.Next(4);
                c = random.Next(4);
                d = random.Next(4);
            }

            tempArray[a, b] = 2;
            tempArray[c, d] = 2;
            //MessageBox.Show(String.Format("{0} {1}", random1.Next(4), random1.Next(4)));

            return tempArray;
        }

        #region Методы-обработчики команд клавиш управления

        private void ExecuteLeftArrowCommand(object obj)
        {
            MessageBox.Show("Left Arrow Command");
        }

        private void ExecuteRightArrowCommand(object obj)
        {
            MessageBox.Show("Right Arrow Command");
        }

        private void ExecuteUpArrowCommand(object obj)
        {
            MessageBox.Show("Up Arrow Command");
        }

        private void ExecuteDownArrowCommand(object obj)
        {
            MessageBox.Show("Down Arrow Command");
        }

        #endregion

        #region Методы-обработчики команд разработчика

        private void ExecuteShowArrayStateCommand(object obj)
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < MainArray.GetLength(0); i++)
            {
                for (int j = 0; j < MainArray.GetLength(1); j++)
                {
                    str.AppendFormat("{0}   ", MainArray[i, j]);
                }

                str.Append("\n");
            }

            MessageBox.Show(str.ToString(), "Main Array State");
        }

        #endregion
    }
}

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
        GameData gameData;
        public GameData GameData
        {
            get
            {
                if(gameData == null)
                {
                    gameData = new GameData(GenerateBeginState());
                }

                return gameData;
            }
            set
            {
                gameData = value;
            }
        }

        public int[,] MainArrayVM
        {
            get
            {
                return GameData.MainArray;
            }
            set
            {
                GameData.MainArray = value;
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

        private int[,] GenerateBeginState()
        {
            int[,] tempArray = new int[4, 4];

            Random random = new Random();

            int a = random.Next(4), b = random.Next(4), 
                c = random.Next(4), d = random.Next(4);

            while (a == c && b == d)
            {
                a = random.Next(4);
                b = random.Next(4);
                c = random.Next(4);
                d = random.Next(4);
            }

            //tempArray[a, b] = 2; // КОРРЕКТНЫЙ КОД!
            //tempArray[c, d] = 2; // КОММЕНТ НА ВРЕМЯ ТЕСТИРОВАНИЯ!
            tempArray[0, 0] = 4;
            tempArray[1, 0] = 2;
            tempArray[1, 3] = 2;
            //MessageBox.Show(String.Format("{0} {1}", random1.Next(4), random1.Next(4)));

            return tempArray;
        }

        #region Методы-обработчики команд клавиш управления

        private void ExecuteLeftArrowCommand(object obj)
        {
            // MessageBox.Show("Left Arrow Command");

            for (int i = 0; i <= MainArrayVM.GetLength(0) - 1; i++) // Итерация строк
            {
                // Сдвиг перед обработкой строки массива
                for (int m = 0; m <= MainArrayVM.GetLength(0) - 1; m++) // Итерация строк
                {
                    for (int n = MainArrayVM.GetLength(1) - 1; n >= 1; n--) // Итерация столбиков
                    {
                        while (MainArrayVM[m, n] != 0 && MainArrayVM[m, n - 1] == 0)
                        {
                            MainArrayVM[m, n - 1] = MainArrayVM[m, n];
                            MainArrayVM[m, n] = 0;
                        }
                    }
                }

                // Слияние влево
                /*
                dontCheck - переключатель для того, чтобы слитое значение не сливалось 
                сразу же с соседним, если равные.
                Т.е. 4<2<2 => 4<4 (один ход) => 8 (следующий)
                */
                bool dontCheck = false;

                for (int j = MainArrayVM.GetLength(1) - 1; j >= 1; j--) // Итерация столбиков
                {
                    if (dontCheck) continue;

                    // Если соседние ненулевые значения равные, то...
                    if (MainArrayVM[i, j] != 0 && MainArrayVM[i, j - 1] != 0 &&
                        MainArrayVM[i, j] == MainArrayVM[i, j - 1])
                    {
                        // Происходит слияние

                        /*
                            ПОДСЧИТАТЬ ОЧКИ!
                        */

                        MainArrayVM[i, j - 1] *= 2;
                        MainArrayVM[i, j] = 0;

                        dontCheck = true;
                    }
                }
            }

            OnPropertyChanged("GameData"); // Вызываем событие для обновления вьюхи
        }

        private void ExecuteRightArrowCommand(object obj)
        {
            MessageBox.Show("Right Arrow Command");


            OnPropertyChanged("GameData");
        }

        private void ExecuteUpArrowCommand(object obj)
        {
            //MessageBox.Show("Up Arrow Command");

            OnPropertyChanged("GameData");
        }

        private void ExecuteDownArrowCommand(object obj)
        {
            //MessageBox.Show("Down Arrow Command");


            OnPropertyChanged("GameData");
        }

        #endregion

        private void ExecuteShowArrayStateCommand(object obj)
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < MainArrayVM.GetLength(0); i++)
            {
                for (int j = 0; j < MainArrayVM.GetLength(1); j++)
                {
                    str.AppendFormat("{0}   ", MainArrayVM[i, j]);
                }

                str.Append("\n");
            }

            MessageBox.Show(str.ToString(), "Main Array State");
        }

    }
}

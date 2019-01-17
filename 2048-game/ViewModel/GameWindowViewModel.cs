using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using _2048_game.Model;
using _2048_game.Infrastructure;
using System.Windows.Input;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace _2048_game.ViewModel
{
    class GameWindowViewModel : ViewModelBase
    {
        private static GameData gameData; // static - костыль для сериализации (или нет)!
        public static bool isContinue = false;
        public GameData GameData
        {
            get
            {
                if(gameData == null)
                {
                    if(!isContinue)
                    {
                        gameData = new GameData(GenerateBeginState());
                        OnPropertyChanged("Score"); // исправление бага, из-за которого в начале игры не отображался нулевой счёт
                    }
                    else
                    {
                        isContinue = false;
                        DeserializationGameData(); // выполняем десериализацию класса GameData
                        OnPropertyChanged("Score"); // исправление бага, из-за которого в начале игры не отображался нулевой счёт
                    }
                }

                return gameData;
            }
            set
            {
                gameData = value;
                OnPropertyChanged("GameData");
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

        public int Score
        {
            get
            {
                return gameData.Score;
            }
            set
            {
                gameData.Score = value;

                OnPropertyChanged("Score");
            }
        }

        public int TheBestScore
        {
            get
            {
                return Properties.Settings.Default.Best_score;
            }
            set
            {
                Properties.Settings.Default.Best_score = value;
                Properties.Settings.Default.Save();

                OnPropertyChanged("TheBestScore");
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

        RelayCommand startOverCommand;
        public ICommand StartOverCommand
        {
            get
            {
                if(startOverCommand == null)
                {
                    startOverCommand = new RelayCommand(ExecuteStartOverCommand);
                }

                return startOverCommand;
            }
        }

        bool dontRepeat = false;

        private void actionAfterArrowCommand()
        {
            OnPropertyChanged("GameData");

            if (Score > Properties.Settings.Default.Best_score)
            {
                TheBestScore = Score;
            }

            int freeCellsCount = 0;

            foreach (var item in MainArrayVM)
            {
                if (item == 2048 && !dontRepeat)
                {
                    dontRepeat = true;
                    MessageBox.Show("Ура! Вы собрали ячейку 2048!", "Победа!");
                }

                if (item == 0)
                {
                    freeCellsCount++;
                }
            }

            if (freeCellsCount < 1)
            {
                OnGameOver();
                return;
            }

            GenerateValueRandomPosition();
        }

        private void OnGameOver()
        {
            MessageBox.Show(String.Format("Игра окончена! Ваш счёт - {0} очков!", Score), "Game over!");
            GameData = null;
            
        }

        private void GenerateValueRandomPosition()
        {
            Random random = new Random();

            while (true)
            {
                int a = random.Next(4), b = random.Next(4);

                if (MainArrayVM[a, b] == 0)
                {
                    int multiplier = random.Next(1, 3);
                    MainArrayVM[a, b] = 2 * multiplier;

                    break;
                }
            }
        }

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

            tempArray[a, b] = 2; // КОРРЕКТНЫЙ КОД!
            tempArray[c, d] = 2; // КОММЕНТ НА ВРЕМЯ ТЕСТИРОВАНИЯ!

            //tempArray[0, 0] = 2;
            //tempArray[1, 0] = 2;
            //tempArray[2, 0] = 2;
            //tempArray[3, 0] = 2;
            
            //MessageBox.Show(String.Format("{0} {1}", random1.Next(4), random1.Next(4)));

            return tempArray;
        }

        #region Методы-обработчики команд клавиш управления

        private void ExecuteLeftArrowCommand(object obj)
        {
            // MessageBox.Show("Left Arrow Command");

            for (int i = 0; i <= MainArrayVM.GetLength(0) - 1; i++) // Итерация строк
            {
                // Слияние влево ПОСЛЕ сдвига
                for (int j = 0; j <= MainArrayVM.GetLength(1) - 1; j++) // Итерация столбиков
                {
                    if (j - 1 < 0) continue; 

                    // Если соседние ненулевые значения равные, то... (выполняем слияние)
                    if (MainArrayVM[i, j] != 0 && MainArrayVM[i, j - 1] != 0 &&
                        MainArrayVM[i, j] == MainArrayVM[i, j - 1])
                    {
                        Score += MainArrayVM[i, j - 1] * 2;

                        MainArrayVM[i, j - 1] *= 2;
                        MainArrayVM[i, j] = 0;
                    }
                }
                // Сдвиг влево ПЕРЕД слиянием
                for (int m = 0; m <= MainArrayVM.GetLength(0) - 1; m++) // Итерация строк
                {
                    for (int n = MainArrayVM.GetLength(1) - 1; n >= 1; n--) // Итерация столбиков
                    {
                        // Если элемент не равен 0, а предыдущий равен 0, то... (сдвигаем влево)
                        if (MainArrayVM[m, n] != 0 && MainArrayVM[m, n - 1] == 0)
                        {
                            MainArrayVM[m, n - 1] = MainArrayVM[m, n];
                            MainArrayVM[m, n] = 0;
                        }
                    }
                }
            }

            actionAfterArrowCommand();
        }

        private void ExecuteRightArrowCommand(object obj)
        {
            //MessageBox.Show("Right Arrow Command");

            for (int i = MainArrayVM.GetLength(0) - 1; i >= 0; i--)  // Итерация строк
            {
                // Слияние вправо ПЕРЕД сдвигом
                for (int j = 0; j <= MainArrayVM.GetLength(1) - 1; j++) // Итерация столбиков
                {
                    if (j + 1 > MainArrayVM.GetLength(1) - 1)
                        continue;

                    // Если соседние ненулевые значения равные, то... (выполняем слияние)
                    if (MainArrayVM[i, j] != 0 && MainArrayVM[i, j + 1] != 0 &&
                        MainArrayVM[i, j] == MainArrayVM[i, j + 1])
                    {
                        Score += MainArrayVM[i, j + 1] * 2;

                        MainArrayVM[i, j + 1] *= 2;
                        MainArrayVM[i, j] = 0;
                    }
                }

                // Сдвиг вправо ПОСЛЕ слияния
                for (int m = MainArrayVM.GetLength(0) - 1; m >= 0; m--) // Итерация строк
                {
                    for (int n = 0; n < MainArrayVM.GetLength(1) - 1; n++) // Итерация столбиков
                    {
                        // Если элемент не равен 0, а следующий равен 0, то... (сдвигаем вправо)
                        if (MainArrayVM[m, n] != 0 && MainArrayVM[m, n + 1] == 0)
                        {
                            MainArrayVM[m, n + 1] = MainArrayVM[m, n];
                            MainArrayVM[m, n] = 0;
                        }
                    }
                }
            }

            actionAfterArrowCommand();
        }

        private void ExecuteUpArrowCommand(object obj)
        {
            //MessageBox.Show("Up Arrow Command");

            
            for (int n = MainArrayVM.GetLength(0) - 1; n > 0; n--) // Итерация строк
            {
                // Слияние
                for (int m = 0; m <= MainArrayVM.GetLength(1) - 1; m++) // Итерация столбиков
                {
                    if (MainArrayVM[n, m] != 0 && MainArrayVM[n - 1, m] != 0 &&
                        MainArrayVM[n, m] == MainArrayVM[n - 1, m])
                    {
                        Score += MainArrayVM[n - 1, m] * 2;

                        MainArrayVM[n - 1, m] *= 2;
                        MainArrayVM[n, m] = 0;
                    }
                }

                // Сдвиг
                for (int i = MainArrayVM.GetLength(0) - 1; i >= 1; i--) // Итерация строк
                {
                    for (int j = 0; j <= MainArrayVM.GetLength(1) - 1; j++) // Итерация столбиков
                    {
                        if (MainArrayVM[i, j] != 0 && MainArrayVM[i - 1, j] == 0)
                        {
                            MainArrayVM[i - 1, j] = MainArrayVM[i, j];
                            MainArrayVM[i, j] = 0;
                        }

                    }
                }
            }

            actionAfterArrowCommand();
        }

        private void ExecuteDownArrowCommand(object obj)
        {
            //MessageBox.Show("Down Arrow Command");

            for (int n = 0; n <= MainArrayVM.GetLength(0) - 1; n++) // Итерация строк
            {
                // Слияние
                for (int m = 0; m <= MainArrayVM.GetLength(1) - 1; m++) // Итерация столбиков
                {
                    if (n + 1 > MainArrayVM.GetLength(1) - 1) continue;

                    if (MainArrayVM[n, m] != 0 && MainArrayVM[n + 1, m] != 0 &&
                        MainArrayVM[n, m] == MainArrayVM[n + 1, m])
                    {
                        Score += MainArrayVM[n + 1, m] * 2;

                        MainArrayVM[n + 1, m] *= 2;
                        MainArrayVM[n, m] = 0;
                    }
                }

                // Сдвиг
                for (int i = 0; i < MainArrayVM.GetLength(0) - 1; i++) // Итерация строк
                {
                    for (int j = MainArrayVM.GetLength(1) - 1; j >= 0; j--) // Итерация столбиков
                    {
                        if (MainArrayVM[i, j] != 0 && MainArrayVM[i + 1, j] == 0)
                        {
                            MainArrayVM[i + 1, j] = MainArrayVM[i, j];
                            MainArrayVM[i, j] = 0;
                        }

                    }
                }
            }

            actionAfterArrowCommand();
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

        private void ExecuteStartOverCommand(object obj)
        {
            GameData = null;
            OnPropertyChanged("GameData");
        }

        /* 
         * В методе SerializationGameData обрабатываются действия при закрытии окна.
         * В нашем случае выполняется сериализация класса GameData.
         * Это нужно для того, чтобы была возможность продолжить игру после закрытия программы.
         */
        private static void SerializationGameData()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            FileStream serializationStream = new FileStream("save.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            serializer.Serialize(serializationStream, gameData);

            serializationStream.Close();

            /* 
             * После выполнения сериализации создаём ключ в реестре системы и присваиваем ему 
             * ЗАШИФРОВАННОЕ значение байтов файла.
             * Делаем это для того, чтобы продолжение игры не запускалось, если файл был изменён вручную.
             * (вероятно, что файл сохранения был изменён в целях накрутки очков)
             */

            FileStream cryptStream = new FileStream("save.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            MD5 md5Hash = MD5.Create();
            StringBuilder source = new StringBuilder();
            byte[] bytes = new byte[cryptStream.Length];

            cryptStream.Read(bytes, 0, (int)cryptStream.Length);

            foreach (var item in bytes)
            {
                source.Append(item);
            }

            Properties.Settings.Default.Save_hash =
                GetMd5Hash(md5Hash, source.ToString());
            Properties.Settings.Default.Save();

            cryptStream.Close();
        }

        /*
         * Метод DeserializationGameData вызывается в том случае, если игрок
         * решил продолжить игру, а не начать новую. 
         * В этом методе происходит десериализация класса GameData.
         */
        private static void DeserializationGameData()
        {
            // Выполняются сверки зашифрованного ключа при сериализации и хэша данных файла при десериализации
            FileStream cryptStream = new FileStream("save.dat", FileMode.Open, FileAccess.Read);
            MD5 md5Hash = MD5.Create();
            StringBuilder source = new StringBuilder();
            byte[] bytes = new byte[cryptStream.Length];

            cryptStream.Read(bytes, 0, (int)cryptStream.Length);
            
            foreach (var item in bytes)
            {
                source.Append(item);
            }

            string hash = Properties.Settings.Default.Save_hash;
            bool isReliably = VerifyMd5Hash(md5Hash, source.ToString(), hash);

            cryptStream.Close();

            if(isReliably)
            {
                FileStream deserializationStream = new FileStream("save.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter deserializer = new BinaryFormatter();

                gameData = deserializer.Deserialize(deserializationStream) as GameData;

                deserializationStream.Close();
            }
            else
            {
                MessageBox.Show("Файл сохранения повреждён, начните игру сначала!", "Критическая ошибка");
            }
        }

        public static void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SerializationGameData();
            gameData = null;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}

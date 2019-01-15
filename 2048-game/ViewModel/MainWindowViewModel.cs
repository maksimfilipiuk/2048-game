using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _2048_game.Infrastructure;
using _2048_game.View;

namespace _2048_game.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        RelayCommand newGameCommand;
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                {
                    newGameCommand = new RelayCommand(ExecuteNewGameCommand);
                }

                return newGameCommand;
            }
        }

        private void ExecuteNewGameCommand(object obj)
        {
            GameWindow gameWindow = new GameWindow("LET`s START!");
            gameWindow.ShowDialog();
        }
    }
}

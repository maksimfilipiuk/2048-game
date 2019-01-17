using System;
using System.Collections.Generic;
using System.IO;
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
        GameWindow gameWindow;

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

        RelayCommand continueGameCommand;
        public ICommand ContinueGameCommand
        {
            get
            {
                if(continueGameCommand == null)
                {
                    continueGameCommand = new RelayCommand(ExecuteContinueGameCommand, CanExecuteContinueGameCommand);
                }

                return continueGameCommand;
            }
        }

        private void ExecuteNewGameCommand(object obj)
        {
            if (gameWindow != null)
                gameWindow = null;

            gameWindow = new GameWindow("Хорошее начало - половина дела!");
            gameWindow.ShowDialog();
        }

        private bool CanExecuteContinueGameCommand(object obj)
        {
            FileInfo saveFile = new FileInfo("save.dat");

            if (!saveFile.Exists)
                return false;

            return true;
        }

        private void ExecuteContinueGameCommand(object obj)
        {
            if (gameWindow != null)
                gameWindow = null;

            gameWindow = new GameWindow("Хорошее начало - половина дела!", true);
            gameWindow.ShowDialog();
        }
    }
}

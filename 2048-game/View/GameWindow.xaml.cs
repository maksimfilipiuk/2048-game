using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using _2048_game.ViewModel;

namespace _2048_game.View
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            
            Closing += GameWindowViewModel.GameWindow_Closing;
        }

        public GameWindow(string title)
            : this()
        {
            Title = title;
        }

        public GameWindow(string title, bool isContinue = false)
        {
            /*
             * Да простят меня миддлы и сеньоры, я нарушил паттерн MVVM (возможно, не один раз).
             * Обработка класса GameWindowViewModel начинается раньше, чем событие Initialized либо Loaded.
             * А при isContinue = false, программа будет начинать новую игру, а не продолжать старую.
             * Я это исправлю... возможно... когда-то... да...
             */
            GameWindowViewModel.isContinue = true;

            InitializeComponent();
            Closing += GameWindowViewModel.GameWindow_Closing;

            Title = title;
        }
    }
}

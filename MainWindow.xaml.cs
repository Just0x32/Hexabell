using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hexabell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region [ Properties and fields ]
        private Thread[] bellThreads;

        public bool[] IsTaskEnabled { get; private set; }

        public Brush[] ButtonColors { get; private set; }

        private readonly Brush DisabledTaskButtonBackground = Brushes.LightSteelBlue;

        private readonly Brush EnabledTaskButtonBackground = Brushes.LightSkyBlue;

        public readonly Brush MouseOverTaskButtonBackground = Brushes.PowderBlue;

        private Dictionary<string, int> taskButtons = new Dictionary<string, int>
        {
            { "FirstTaskButton", 0 },
            { "SecondTaskButton", 1 },
            { "ThirdTaskButton", 2 },
            { "FourthTaskButton", 3 },
            { "FifthTaskButton", 4 },
            { "SixthTaskButton", 5 },
        };
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            InitializeTaskButtons();

            void InitializeTaskButtons()
            {
                int tasksQuantity = 6;
                bellThreads = new Thread[tasksQuantity];
                IsTaskEnabled = new bool[tasksQuantity];
                ButtonColors = new Brush[tasksQuantity];

                for (int i = 0; i < tasksQuantity; i++)
                {
                    IsTaskEnabled[i] = false;
                    ButtonColors[i] = DisabledTaskButtonBackground;
                }

                FirstTaskButton.Background = ButtonColors[0];
                SecondTaskButton.Background = ButtonColors[1];
                ThirdTaskButton.Background = ButtonColors[2];
                FourthTaskButton.Background = ButtonColors[3];
                FifthTaskButton.Background = ButtonColors[4];
                SixthTaskButton.Background = ButtonColors[5];
            }
        }

        private void BasicPolygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void TaskButton_Click(object sender, RoutedEventArgs e) => ChangeTaskButtonState(sender as Button);

        private void ChangeTaskButtonState(Button taskButton)
        {
            int index = taskButtons[taskButton.Name];

            IsTaskEnabled[index] = !IsTaskEnabled[index];

            if (IsTaskEnabled[index])
            {
                StartBellTask(index);
                ButtonColors[index] = EnabledTaskButtonBackground;
            }
            else
            {
                EndBellTask(index);
                ButtonColors[index] = DisabledTaskButtonBackground;
            }

            taskButton.Background = ButtonColors[index];
        }

        private void StartBellTask(int taskIndex)
        {

        }

        private void EndBellTask(int taskIndex)
        {

        }

        private bool IsNoError()
        {
            return false;
        }
    }
}

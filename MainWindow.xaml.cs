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
        ViewModel viewModel;

        public bool[] IsTaskEnabled { get => viewModel.IsTaskEnabled; }

        public Brush[] ButtonColors { get; private set; }

        private readonly Brush disabledTaskButtonBackground = Brushes.LightSteelBlue;

        private readonly Brush enabledTaskButtonBackground = Brushes.LightSkyBlue;

        private readonly Brush mouseOverTaskButtonBackground = Brushes.PowderBlue;

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

            int taskQuantity = 6;
            viewModel = new ViewModel(taskQuantity);
            InitializeTaskButtons(taskQuantity);

            void InitializeTaskButtons(int taskQuantity)
            {
                ButtonColors = new Brush[taskQuantity];

                for (int i = 0; i < taskQuantity; i++)
                {
                    ButtonColors[i] = disabledTaskButtonBackground;
                }

                SetTaskButtonColor(FirstTaskButton);
                SetTaskButtonColor(SecondTaskButton);
                SetTaskButtonColor(ThirdTaskButton);
                SetTaskButtonColor(FourthTaskButton);
                SetTaskButtonColor(FifthTaskButton);
                SetTaskButtonColor(SixthTaskButton);
            }
        }

        private int IndexOfTask(Button taskButton)
        {
            return taskButtons[taskButton.Name];
        }

        private void SetTaskButtonColor(Button taskButton)
        {
            int index = IndexOfTask(taskButton);

            if (IsTaskEnabled[index])
            {
                ButtonColors[index] = enabledTaskButtonBackground;
            }
            else
            {
                ButtonColors[index] = disabledTaskButtonBackground;
            }

            taskButton.Background = ButtonColors[index];
        }

        private void ChangeTaskState(Button taskButton)
        {
            int index = IndexOfTask(taskButton);

            viewModel.ChangeTaskState(index);
            SetTaskButtonColor(taskButton);
        }

        

        private bool IsNoError()
        {
            return false;
        }

        #region [ Handlers ]
        private void BasicPolygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void TaskButton_Click(object sender, RoutedEventArgs e) => ChangeTaskState(sender as Button);
        #endregion
    }
}

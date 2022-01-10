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

        public Brush[] ButtonBackgroundColors { get; private set; }

        public Brush[] ButtonBorderBrushColors { get; private set; }

        private readonly Brush disabledTaskButtonBackground = new SolidColorBrush(Color.FromArgb(93, 135, 206, 250));       // LightSkyBlue

        private readonly Brush disabledTaskButtonBorderBrush = new SolidColorBrush(Color.FromArgb(120, 135, 206, 250));       // LightSkyBlue

        private readonly Brush enabledTaskButtonBackground = new SolidColorBrush(Color.FromArgb(93, 255, 182, 193));        // LightPink

        private readonly Brush enabledTaskButtonBorderBrush = new SolidColorBrush(Color.FromArgb(120, 255, 182, 193));         // LightPink

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

            BasicPolygon.Fill = disabledTaskButtonBackground;
            BasicPolygon.Stroke = disabledTaskButtonBorderBrush;

            void InitializeTaskButtons(int taskQuantity)
            {
                ButtonBackgroundColors = new Brush[taskQuantity];
                ButtonBorderBrushColors = new Brush[taskQuantity];

                for (int i = 0; i < taskQuantity; i++)
                {
                    ButtonBackgroundColors[i] = disabledTaskButtonBackground;
                    ButtonBorderBrushColors[i] = disabledTaskButtonBorderBrush;
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
                ButtonBackgroundColors[index] = enabledTaskButtonBackground;
                ButtonBorderBrushColors[index] = enabledTaskButtonBorderBrush;
            }
            else
            {
                ButtonBackgroundColors[index] = disabledTaskButtonBackground;
                ButtonBorderBrushColors[index] = disabledTaskButtonBorderBrush;
            }

            taskButton.Background = ButtonBackgroundColors[index];
            taskButton.BorderBrush = ButtonBorderBrushColors[index];
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static readonly int taskQuantity = 6;
        public ViewModel viewModel = new ViewModel();

        private Dictionary<Button, int> indexFromButton = new Dictionary<Button, int>();
        private Dictionary<int, Button> buttonFromIndex = new Dictionary<int, Button>();

        public int ForMouseClickTime { get; private set; } = 200;       // From settings

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

            InitializeTaskButtonIndexesAndColors(taskQuantity);

            void InitializeTaskButtonIndexesAndColors(int taskQuantity)
            {
                buttonFromIndex.Add(0, FirstTaskButton);
                buttonFromIndex.Add(1, SecondTaskButton);
                buttonFromIndex.Add(2, ThirdTaskButton);
                buttonFromIndex.Add(3, FourthTaskButton);
                buttonFromIndex.Add(4, FifthTaskButton);
                buttonFromIndex.Add(5, SixthTaskButton);

                for (int i = 0; i < taskQuantity; i++)
                {
                    indexFromButton.Add(buttonFromIndex[i], i);
                }
            }
        }

        private void ChangeTaskState(Button taskButton)
        {
            int index = indexFromButton[taskButton];

            viewModel.ChangeTaskState(index);
        }

        private void ChangeTaskOptions(Button taskButton)
        {
            int index = indexFromButton[taskButton];
            string soundPath = viewModel.SoundPaths[index];
            string time = viewModel.TaskTimes[index];

            TaskWindow taskWindow = new TaskWindow(time, soundPath);
            taskWindow.Owner = this;

            if (taskWindow.ShowDialog() == true)
            {
                MessageBox.Show(taskWindow.Hours + ":" + taskWindow.Minutes + Environment.NewLine + taskWindow.SoundPath);   // Debug

                // Send time to ViewModel
                // Send other options to ViewModel
            }
        }

        private bool IsNoError()
        {
            return false;           // Make viewModel.IsNoError()
        }

        #region [ Task Button Click Handler ]
        private void TaskButtonClickHandler(object sender, int clickCount)
        {
            Button button = sender as Button;

            if (clickCount > 1)
            {
                CancelTaskStateChanging(button);
                ChangeTaskOptions(button);
            }

            void CancelTaskStateChanging(Button button) => ChangeTaskState(button);
        }

        private delegate void ClickCountHandler(object sender, int clickCount);
        private int ClickCount { get; set; } = 0;
        private bool IsClickCounterTimerStarted = false;
        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            ClickCounter();

            void ClickCounter()
            {
                if (ClickCount == 0)
                {
                    ClickCount += 1;
                    PreStartTask();
                    StartClickCounterTimer();
                }
                else if (IsClickCounterTimerStarted)
                {
                    ClickCount += 1;
                }

                void PreStartTask() => ChangeTaskState(sender as Button);

                void StartClickCounterTimer()
                {
                    IsClickCounterTimerStarted = true;

                    Thread thread = new Thread(new ParameterizedThreadStart(StopClickCounterTimerWithDelay));
                    thread.Start(sender);

                    void StopClickCounterTimerWithDelay(object sender)
                    {
                        Thread.Sleep(ForMouseClickTime);
                        IsClickCounterTimerStarted = false;

                        ClickCountHandler clickCountHandler = new ClickCountHandler(TaskButtonClickHandler);
                        this.Dispatcher.Invoke(clickCountHandler, new object[] { sender, ClickCount });

                        ClickCount = 0;
                    }
                }
            }
        }
        #endregion

        private void BasicPolygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

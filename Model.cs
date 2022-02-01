using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IOExtension;

namespace Hexabell
{
    public class Model : INotifyPropertyChanged
    {
        private readonly int taskQuantity;
        private readonly string defaultTaskHour = "00";
        private readonly string defaultTaskMinute = "00";
        private readonly char defaultTimeSeparator = ':';

        private string[] taskTimes;
        public string[] TaskTimes
        {
            get => taskTimes;
            private set
            {
                if (taskTimes != value)
                    taskTimes = value;

                OnPropertyChanged();
            }
        }

        private string[] soundPaths;
        public string[] SoundPaths
        {
            get => soundPaths;
            private set
            {
                if (soundPaths != value)
                    soundPaths = value;

                OnPropertyChanged();
            }
        }

        private bool[] isTaskEnabled;
        public bool[] IsTaskEnabled
        {
            get => isTaskEnabled;
            private set
            {
                if (isTaskEnabled != value)
                    isTaskEnabled = value;

                OnPropertyChanged();
            }
        }

        private Thread[] bellThreads;

        public Model(int taskQuantity)
        {
            this.taskQuantity = taskQuantity;
            InitializeTaskArrays();

            void InitializeTaskArrays()
            {
                IsTaskEnabled = new bool[taskQuantity];
                TaskTimes = new string[taskQuantity];
                SoundPaths = new string[taskQuantity];
                bellThreads = new Thread[taskQuantity];

                for (int i = 0; i < taskQuantity; i++)
                {
                    IsTaskEnabled[i] = false;
                    TaskTimes[i] = defaultTaskHour + defaultTimeSeparator + defaultTaskMinute;
                }
            }
        }

        public void ChangeTaskState(int taskIndex)
        {
            IsTaskEnabled[taskIndex] = !IsTaskEnabled[taskIndex];
            TaskStateChangedNotify();

            void TaskStateChangedNotify() => IsTaskEnabled = IsTaskEnabled;
        }

        public void SetTaskTime(int taskIndex, string inputTaskTime)
        {
            if (AreValidValues())
            {
                TaskTimes[taskIndex] = inputTaskTime;
                TaskTimes = TaskTimes;
            }

            bool AreValidValues()
            {
                if (!string.IsNullOrEmpty(inputTaskTime))
                {
                    if (inputTaskTime.Length == 4)
                        inputTaskTime = "0" + inputTaskTime;

                    if (inputTaskTime.Length == 5)
                    {
                        int minutes;
                        int hours;

                        if (int.TryParse(inputTaskTime[0..1], out hours) && inputTaskTime[2] == defaultTimeSeparator && int.TryParse(inputTaskTime[3..4], out minutes))
                        {
                            if (hours < 24 && minutes < 60)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }

        public void SetSoundPath(int taskIndex, string soundPath)
        {
            SoundPaths[taskIndex] = soundPath;
            SoundPaths = SoundPaths;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

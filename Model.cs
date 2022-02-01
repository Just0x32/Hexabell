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
        public int[] Hours { get; private set; }
        public int[] Minutes { get; private set; }
        public string[] TaskTimes { get => default; private set => OnPropertyChanged(); }

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
                Hours = new int[taskQuantity];
                Minutes = new int[taskQuantity];
                SoundPaths = new string[taskQuantity];
                bellThreads = new Thread[taskQuantity];
            }
        }

        public void ChangeTaskState(int taskIndex)
        {
            IsTaskEnabled[taskIndex] = !IsTaskEnabled[taskIndex];
            TaskStateChangedNotify();

            void TaskStateChangedNotify() => IsTaskEnabled = IsTaskEnabled;
        }

        public void SetTaskTime(int taskIndex, int hour, int minute)
        {
            if (IsValid(taskIndex, taskQuantity) && IsValid(hour, 23) && IsValid(minute, 59))
            {
                Hours[taskIndex] = hour;
                Minutes[taskIndex] = minute;
                TaskTimes = TaskTimes;
            }
        }

        public void SetSoundPath(int taskIndex, string soundPath)
        {
            if (IsValid(taskIndex, taskQuantity) && IsValid(soundPath, 3))
            {
                SoundPaths[taskIndex] = soundPath;
                SoundPaths = SoundPaths;
            }
        }

        private bool IsValid(int value, int maxValue, int minValue = 0)
        {
            if (value >= 0 && value <= maxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsValid(string value, int minLength = 0)
        {
            if (!string.IsNullOrEmpty(value) && value.Length >= minLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

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

        private Thread[] bellThreads;

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

        public Model(int taskQuantity)
        {
            this.taskQuantity = taskQuantity;
            InitializeTaskArrays();

            void InitializeTaskArrays()
            {
                IsTaskEnabled = new bool[taskQuantity];
                bellThreads = new Thread[taskQuantity];

                for (int i = 0; i < taskQuantity; i++)
                    IsTaskEnabled[i] = false;
            }
        }

        public void ChangeTaskState(int taskIndex)
        {
            IsTaskEnabled[taskIndex] = !IsTaskEnabled[taskIndex];
            TaskStateChangedNotify();

            void TaskStateChangedNotify() => IsTaskEnabled = IsTaskEnabled;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

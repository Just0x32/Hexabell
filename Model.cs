using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hexabell
{
    public class Model
    {
        #region [ Fields and properties ]
        private readonly int taskQuantity;

        private Thread[] bellThreads;

        public bool[] IsTaskEnabled { get; private set; }
        #endregion

        public Model(int taskQuantity)
        {
            this.taskQuantity = taskQuantity;
            InitializeTaskArrays();

            void InitializeTaskArrays()
            {
                IsTaskEnabled = new bool[taskQuantity];
                bellThreads = new Thread[taskQuantity];

                for (int i = 0; i < taskQuantity; i++)
                {
                    IsTaskEnabled[i] = false;
                }
            }
        }

        public void ChangeTaskState(int taskIndex)
        {
            IsTaskEnabled[taskIndex] = !IsTaskEnabled[taskIndex];
        }
    }
}

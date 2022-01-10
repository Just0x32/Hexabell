using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexabell
{
    public class ViewModel
    {
        Model model;

        public bool[] IsTaskEnabled { get => model.IsTaskEnabled; }

        public ViewModel(int taskQuantity) => model = new Model(taskQuantity);

        public void ChangeTaskState(int taskIndex) => model.ChangeTaskState(taskIndex);
    }
}

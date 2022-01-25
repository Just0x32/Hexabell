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

namespace Hexabell
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public string Time { get; private set; }

        public TaskWindow(Button taskButton)
        {
            InitializeComponent();
            DataContext = this;
            Time = taskButton.Content.ToString();
            MessageBox.Show(Time);
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void TimeButton_Click(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = new DatePicker();
        }
    }
}

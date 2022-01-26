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
        private List<string> minutes = new List<string>();
        private List<string> hours = new List<string>();
        private string separator;

        public string Time { get; private set; }

        public TaskWindow(Button taskButton)
        {
            InitializeComponent();
            DataContext = this;

            GetInputTime();

            void GetInputTime()
            {
                InitializeHoursComboBox(taskButton.Content.ToString()[..2]);
                InitializeMinutesComboBox(taskButton.Content.ToString()[^2..]);
                InitializeSeparator(taskButton.Content.ToString()[2].ToString());
            }
        }

        private void InitializeHoursComboBox(string inputHours)
        {
            for (int i = 0; i < 24; i++)
            {
                hours.Add(string.Format("{0:d2}", i));
            }

            HoursComboBox.ItemsSource = hours;
            HoursComboBox.SelectedValue = inputHours;
        }

        private void InitializeMinutesComboBox(string inputMinutes)
        {
            for (int i = 0; i < 60; i++)
            {
                minutes.Add(string.Format("{0:d2}", i));
            }

            MinutesComboBox.ItemsSource = minutes;
            MinutesComboBox.SelectedValue = inputMinutes;
        }

        private void InitializeSeparator(string inputSeparator)
        {
            separator = inputSeparator;
            SeparatorTextBlock.Text = separator;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            string outputHours = HoursComboBox.SelectedValue as string;
            string outputMinutes = MinutesComboBox.SelectedValue as string;
            Time = outputHours + separator + outputMinutes;

            this.DialogResult = true;
        }
    }
}

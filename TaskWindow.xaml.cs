using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class TaskWindow : Window, INotifyPropertyChanged
    {
        public string Hours { get; set; }
        public string Minutes { get; set; }

        private string soundPath;
        public string SoundPath
        {
            get => soundPath;
            private set
            {
                soundPath = value;
                OnPropertyChanged();
            }
        }

        public TaskWindow(string time, string soundPath)
        {
            InitializeComponent();
            DataContext = this;

            Hours = time[..2];
            Minutes = time[^2..];
            SoundPath = soundPath;
        }

        private void Accept_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;

        private void ChangeSoundPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
                SoundPath = openFileDialog.FileName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged ([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

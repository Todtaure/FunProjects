using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Spritz_HomemadeVersion.Annotations;

namespace Spritz_HomemadeVersion
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DialogueWindow : Window, INotifyPropertyChanged
    {
        public List<string> ReadingSpeed { get; set; }

        public string WordToRead
        {
            get { return _wordToRead; }
            set
            {
                _wordToRead = value;
                OnPropertyChanged();
            }
        }

        public string PauseButtonText
        {
            get { return _pauseButtonText; }
            set
            {
                _pauseButtonText = value;
                OnPropertyChanged();
            }
        }

        private int wordCount = 0;
        private string _wordToRead;
        private string _pauseButtonText;

        public DialogueWindow()
        {
            InitializeComponent();
            ReadingSpeed = new List<string>() { "400", "500", "600" };
            PauseButtonText = "Go!";
            WordToRead = "Press to start....";
        }
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
           this.Close();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButtonText = PauseButton.Content.Equals("Go!") ? "Pause" : "Go!";
            
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (Sender, f) => DisplayWords(Sender, f, timer);
            //timer.Elapsed += new ElapsedEventHandler(DisplayWords);
            timer.AutoReset = true;
            timer.Start();
        }

        private void DisplayWords(object source, ElapsedEventArgs e, System.Timers.Timer timer)
        {
            if (wordCount >= WordReader.WordList.Count)
            {
                timer.Enabled = false;
                wordCount = 0;
                PauseButtonText = "Go!";
                timer.Stop();
                MessageBox.Show("Fin");
            }
            else
            {
                WordToRead = WordReader.WordList.ElementAt(wordCount);
                wordCount++;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Spritz_HomemadeVersion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _textReadInput = null;
        
        public string TextReadInput
        {
            get { return _textReadInput; }
            set { _textReadInput = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WordReader.InitializeTextReaderList(TextReadInput);

            var readerWindow = new DialogueWindow();

            readerWindow.Owner = this;
            readerWindow.Show();

        }
    }

    public class WordReader
    {
        public static List<string> WordList; 
        public static void InitializeTextReaderList(string str)
        {
            if (!str.Equals(""))
            {
                char[] separator = { ' ' };
                WordList = new List<string>();
                foreach (var word in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                {
                    WordList.Add(word);
                }
            }
        }
    }

}

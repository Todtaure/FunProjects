using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;


namespace FileRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isPathSet;
        int n;
        public MainWindow()
        {
            InitializeComponent();
            _isPathSet = false;
            Version.Text = "Version 1.3.4.0";
        }

        /// <summary>
        /// Convert file names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isPathSet)
            {
                if (RegexRadioButton.IsChecked == true)
                {
                    var fullPaths = Directory.EnumerateFiles(SourceBox.Text, "*.mp3");
                    
                    var fullPaths1 = fullPaths as IList<string> ?? fullPaths.ToList();
                    var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(SourceBox.Text, "*.mp3")
                                     select Path.GetFileName(fullMusicFiles);
                    var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();
                    for (int i = 0; i < enumerable.Count(); i++)
                    {
                        if (!enumerable[i].Contains(AddBox.Text))
                        {
                            var newFileName = Regex.Replace(enumerable[i], DeleteBox.Text, string.Empty); //\d-\d
                            newFileName = Regex.Replace(newFileName, ".mp", AddBox.Text + ".mp3"); // - Audiomachine

                            var test = SourceBox.Text + "\\" + newFileName;
                            var paths = fullPaths as IList<string> ?? fullPaths1.ToList();
                            File.Move(paths.ElementAt(i), test);
                        }
                    }  
                }
                else if (CharRadioButton.IsChecked == true && int.TryParse(StartChar.Text, out n) && int.TryParse(EndChar.Text, out n))
                {
                    var fullPaths = Directory.EnumerateFiles(SourceBox.Text, "*.mp3");
                    var fullPaths1 = fullPaths as IList<string> ?? fullPaths.ToList();
                    var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(SourceBox.Text, "*.mp3")
                        select Path.GetFileName(fullMusicFiles);
                    var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();
                    int exceptionsCounter = 1;

                    for (int i = 0; i < enumerable.Count(); i++)
                    {
                        if (!enumerable[i].Contains(AddBox.Text))
                        {
                            var newFileName = enumerable[i].Remove(0, Int32.Parse(StartChar.Text));
                            newFileName = newFileName.Remove(newFileName.Length - Int32.Parse(EndChar.Text) - 4,
                                Int32.Parse(EndChar.Text));

                            try
                            {
                                newFileName = Regex.Replace(newFileName.Trim(), ".mp3", AddBox.Text + ".mp3");
                                var test = SourceBox.Text + "\\" + newFileName;
                                var paths = fullPaths as IList<string> ?? fullPaths1.ToList();
                                File.Move(paths.ElementAt(i), test);
                                exceptionsCounter = 1;
                            }
                            catch (IOException exception)
                            {
                                if (exception.Message == "Cannot create a file when that file already exists.\r\n")
                                {
                                    newFileName = Regex.Replace(newFileName, AddBox.Text + ".mp3",
                                        AddBox.Text + "0" + exceptionsCounter + ".mp3");
                                    var test = SourceBox.Text + "\\" + newFileName;
                                    var paths = fullPaths as IList<string> ?? fullPaths1.ToList();
                                    File.Move(paths.ElementAt(i), test);
                                    exceptionsCounter++;
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Char value must be positiv integer!");
                }
            }
        }

        /// <summary>
        /// Tjek filepath
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SourceBox.Text = fbd.SelectedPath;
                _isPathSet = true;
            }
        }

        /// <summary>
        /// Test function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (_isPathSet)
            {
                if (RegexRadioButton.IsChecked == true)
                {
                    var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(SourceBox.Text, "*.mp3", SearchOption.AllDirectories)
                                     select Path.GetFileName(fullMusicFiles);
                    var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();

                    TestBlock.Text = enumerable.ElementAt(0);

                    if (!enumerable.ElementAt(0).Contains(AddBox.Text))
                    {
                        var newFileName = Regex.Replace(enumerable.ElementAt(0), DeleteBox.Text, string.Empty); //"\d-\d"

                        newFileName = Regex.Replace(newFileName.Trim(), ".mp", AddBox.Text + ".mp3"); // - Audiomachine
                        ResultTestBlock.Text = newFileName;
                    }
                    else
                    {
                        ResultTestBlock.Text = TestBlock.Text;
                    }  
                }
                else if (CharRadioButton.IsChecked == true && int.TryParse(StartChar.Text, out n) && int.TryParse(EndChar.Text, out n))
                {
                    var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(SourceBox.Text, "*.mp3", SearchOption.AllDirectories)
                                     select Path.GetFileName(fullMusicFiles);
                    var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();

                    TestBlock.Text = enumerable.ElementAt(0);

                    if (!enumerable.ElementAt(0).Contains(AddBox.Text))
                    {
                        var newFileName = enumerable[0].Remove(0, Int32.Parse(StartChar.Text));
                        newFileName = newFileName.Remove(newFileName.Length - Int32.Parse(EndChar.Text) - 4,
                            Int32.Parse(EndChar.Text));

                        newFileName = Regex.Replace(newFileName, ".mp3", AddBox.Text + ".mp3");
                        ResultTestBlock.Text = newFileName;
                    }
                    else
                    {
                        ResultTestBlock.Text = TestBlock.Text;
                    }  
                }
            }
        }

        private void CheckPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SourceBox.Text))
            {
                _isPathSet = false;
                return;
            }

            _isPathSet = true;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.ComponentModel;

namespace FileRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FileController _fileController;
        private bool _isPathSet;

        public MainWindow()
        {
            InitializeComponent();
            _isPathSet = false;
            SourceBox.Foreground = Brushes.Red;
            Version.Text = "Version 1.6.0.0";

            _fileController = new FileController();
            
            var bgWorker = new BackgroundWorker();
            bgWorker.DoWork +=
                (sender, args) =>
                    System.Windows.MessageBox.Show("This version may contain bugs!", "File renamer says...",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
            bgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Convert file names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            int start, end;
            var bgWorker = new BackgroundWorker();
            if (ReplaceRadioButton.IsChecked == true && !String.IsNullOrEmpty(DeleteBox.Text))
            {
                bgWorker.DoWork += (o, args) => _fileController.ReplaceStrings(DeleteBox.Text, ReplaceBox.Text);
                bgWorker.RunWorkerAsync();
            }
            else if (CharRadioButton.IsChecked == true && Int32.TryParse(EndChar.Text, out end) && Int32.TryParse(StartChar.Text, out start))
            {
                bgWorker.DoWork += (o, args) => _fileController.InsertAndRemove(AddBox.Text, start, end);
                bgWorker.RunWorkerAsync();
            }
            else
            {
                System.Windows.MessageBox.Show("Char value must be positiv integer!", "FileRenamer says...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Check filepath
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseFolders_Click(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SourceBox.Text = _fileController.CheckFilePath(fbd.SelectedPath) ? fbd.SelectedPath : "";
            }
        }

        /// <summary>
        /// Test function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestRename_Click(object sender, RoutedEventArgs e)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.DoWork += (o, args) => TestRenameWorker();
            bgWorker.RunWorkerAsync();
        }

        private void TestRenameWorker()
        {
            int start, end;
            string testBlockOut = null, resultBlockOut = null;
            if (ReplaceRadioButton.IsChecked == true && !String.IsNullOrEmpty(DeleteBox.Text))
            {
                resultBlockOut = _fileController.TestReplaceStrings(DeleteBox.Text, ReplaceBox.Text, out testBlockOut);

            }
            else if (CharRadioButton.IsChecked == true && int.TryParse(EndChar.Text, out end) && int.TryParse(StartChar.Text, out start))
            {
                resultBlockOut = _fileController.TestInsertAndRemove(ReplaceBox.Text, start, end, out testBlockOut);
            }

            if (testBlockOut == null || resultBlockOut == null)
            {
                return;
            }

            TestBlock.Text = testBlockOut;
            ResultTestBlock.Text = resultBlockOut;
        }

        private void CheckPathButton_OnClick(object sender, RoutedEventArgs e)
        {
            CheckFolderPath();
        }

        private void CheckFolderPath()
        {
            _isPathSet = _fileController.CheckFilePath(SourceBox.Text);

            SourceBox.Foreground = _isPathSet ? Brushes.Green : Brushes.Red;

            if (!_isPathSet)
                System.Windows.MessageBox.Show("Folder check failed!\n The folder doesn´t exist or\n does not contain any .mp3 files.", "FileRenamer says...",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Capitalize_OnClick(object sender, RoutedEventArgs e)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.DoWork += (o, args) => _fileController.CapitalizeAll();
            bgWorker.RunWorkerAsync();
        }

    }
}

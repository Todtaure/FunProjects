using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace Wallpaper_viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _textBoxNamePreviewList;
        private readonly string _previewName;
        private string _wallpaperDirectory, _resolution;
        private bool _resolutionSearch, _groupSearch, _vectorSearch;

        public MainWindow()
        {
            InitializeComponent();

            //Randomize Preview names in searchbox
            CreateNameList();
            var names = new Random();
            _previewName = _textBoxNamePreviewList.ElementAt(names.Next(11));
            SearchBox.Text = _previewName;
            //END RANDOMIZE PREVIEV NAMES

            //Setup wallpaper folder path box
            FolderDirectoryBox.Text = string.Format(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Pictures");
            //END WALLPAPER FOLDER PATH BOX SETUP

            //Setup textblocks
            FolderCount.Text = "Folders in direcotry:";
            ItemCount.Text = "Files in directory:";
            FileMatchBlock.Text = "Files matching search:";
            //END SETUP TEXTBLOCKS

            //Setup advanced settings
            _resolutionSearch = false;
            _groupSearch = false;
            _vectorSearch = false;
            //Resolution Box setup
            var tmp = new List<string>
            {
                "below 1080p",
                "1920x1080 (16:9)",
                "1920x1200 (16:10)",
                "2560x1440 (16:9)",
                "2560x1600 (16:10)",
                "3840x2160 (4k)",
                "Other than 16:9 / 16:10"
            };

            foreach (var tmpRes in tmp)
            {
                ResolutionBox.Items.Add(tmpRes);
            }
            //END ADVANCED SETTINGS
        }

        private void SearchBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Remove gray preview text in Search box
            if (sender is TextBox)
            {
                if (((TextBox) sender).Foreground == Brushes.Gray)
                {
                    ((TextBox) sender).Text = "";
                    ((TextBox) sender).Foreground = Brushes.Black;
                }
            }
        }

        private void SearchBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Reset previewname if none other is entered
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Text = _previewName;
                    ((TextBox)sender).Foreground = Brushes.Gray;
                }
            }
        }

        private void CreateNameList()
        {
            //namelist for previewnames in searchbox
            _textBoxNamePreviewList = new List<string>
            {
                "Twilight Sparkle",
                "Rainbow Dash",
                "Applejack",
                "Fluttershy",
                "Pinkie Pie",
                "Rarity",
                "Spike",
                "Sweetie Belle",
                "Scootaloo",
                "Apple Bloom",
                "Celestia",
                "Luna"
            };

        }

        private void FolderDirectoryBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Remove gray preview text in direcotry box
            if (sender is TextBox)
            {
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }

        private void FolderDirectoryBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Reset previewname if none other is entered
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Text = string.Format(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Pictures");
                    ((TextBox)sender).Foreground = Brushes.Gray;
                }
            }
        }

        private void SetupImageList()   //IMPLEMENT NEXT
        {
            var testImage = new BitmapImage();
            var imageTest = new Image();


            string pathName = string.Format(_wallpaperDirectory + "\\" + SearchBox.Text + "\\1920x1200(16-10)");

            foreach (var files in Directory.GetFiles(@pathName))
            {
                testImage.UriSource = new Uri(files);
            }
        }

        private void LoadGalleryButton_Click(object sender, RoutedEventArgs e)
        {
            _wallpaperDirectory = _wallpaperDirectory ?? Directory.GetCurrentDirectory();
            var folderCount = Directory.EnumerateDirectories(_wallpaperDirectory);
            FolderCount.Text = string.Format("Folders in direcotry: {0}", folderCount.Count());
            var itemCount = Directory.GetFiles(_wallpaperDirectory, "*.*", SearchOption.AllDirectories);
            ItemCount.Text = string.Format("Files in directory: {0}", itemCount.Count());
            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Implementation missing ...");
            SearchMatchCount();
        }

        private void SearchMatchCount()
        {
            var searchMatchCount = Directory.GetFiles(string.Format(_wallpaperDirectory + "\\" + SearchBox.Text), "*.*", SearchOption.AllDirectories);
            FileMatchBlock.Text = string.Format("Files matching search: {0}", searchMatchCount.Count());
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderDirectoryBox.Text = _wallpaperDirectory = fbd.SelectedPath;
                FolderDirectoryBox.Foreground = Brushes.Black;
            }
        }

        private void GroupCheckBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _groupSearch = GroupCheckBox.IsChecked == true;
        }

        private void ResolutionSearchCheckBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_resolutionSearch = (ResolutionSearchCheckBox.IsChecked == true))
            {
                _resolution = ResolutionBox.Text;
            }
            else
            {
                _resolution = "";
            }
        }

        private void VectorCheckBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _vectorSearch = VectorCheckBox.IsChecked == true;
        }

    }
}

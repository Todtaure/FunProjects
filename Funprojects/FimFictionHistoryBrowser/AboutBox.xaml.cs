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

namespace FimFictionHistoryBrowser
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();

            AuthorBlock.Text = "Author:\n" +
                               "Sebastian \"Todtaure\" Breuer";
            DescriptionBlock.Text = "Description:\n" +
                                    "FiMReader© is intended for managing E-Books. " +
                                    "Currently only a local database is available, but an online solution will be targeted in the future. " +
                                    "Also to be added are features like: Read Story button, Go to story/author page etc. " +
                                    "Should there be any problems or suggestions feel free to e-mail me.";
            EmailBlock.Text = "razak.todtaure@gmail.com";
            EmailBlock.Foreground = new SolidColorBrush(Color.FromRgb(2,68,255));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EmailBlock_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "mailto:razak.todtaure@gmail.com?subject=FimReader";
            proc.Start();
        }
    }
}

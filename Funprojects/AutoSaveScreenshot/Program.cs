using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Clipboard = System.Windows.Clipboard;

namespace AutoSaveScreenshot
{
    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();
            
            string photolocation = rand.Next(0, 500) + " - " + rand.Next(0, 65000) + ".png";

            string location = @"C:\Users\" + Environment.UserName + @"\Pictures\" + photolocation;

            while (File.Exists(location))
            {
                photolocation = rand.Next(0, 500) + " - " + rand.Next(0, 65000) + ".png";

                location = @"C:\Users\" + Environment.UserName + @"\Pictures\" + photolocation;
            }

            var stream = new FileStream(location, FileMode.CreateNew, FileAccess.Write);

            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(printscreen as Image);

            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            printscreen.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

            MessageBox.Show("File saved under:" + location);
        }
    }
}

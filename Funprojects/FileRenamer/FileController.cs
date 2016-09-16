using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FileRenamer
{
    public class FileController
    {
        private bool _isPathValid;
        private string _folderURL;
        private string _fileType;
        private ProgressBar pgb;

        public FileController(ProgressBar pgb)
        {
            _folderURL = null;
            _isPathValid = false;
            _fileType = ".mp3";
            this.pgb = pgb;
        }

        public bool CheckFilePath(string path)
        {
            _isPathValid = false;
            if (Directory.Exists(path) && Directory.EnumerateFiles(path, "*" + _fileType).Any())
            {
                _folderURL = path;
                _isPathValid = true;
            }
            return _isPathValid;
        }

        public void SetFileType(string fileType)
        {
            _fileType = "."+fileType;
        }

        public void ReplaceStrings(string removeValue, string replaceValue)
        {
            if (!_isPathValid)
            {
                MessageBox.Show("Folder check failed!\n The folder doesn´t exist or\n does not contain any .mp3 files.", "File renamer says...",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var fullPaths = Directory.EnumerateFiles(_folderURL, "*" + _fileType);

            var fullPaths1 = fullPaths as IList<string> ?? fullPaths.ToList();
            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*" + _fileType, SearchOption.TopDirectoryOnly)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();

            var cnt = enumerable.Count();
            
            pgb.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate()
            {
                pgb.Value = 0;
                pgb.Maximum = cnt;
            }));

            for (int i = 0; i < cnt; i++)
            {
                var newFileName = enumerable.ElementAt(i).Replace(removeValue, replaceValue);
                var newPath = _folderURL + "\\" + newFileName;
                var paths = fullPaths as IList<string> ?? fullPaths1.ToList();
                File.Move(paths.ElementAt(i), newPath);

                increaseProgress(i);
            }
        }

        public void InsertAndRemove(string insertValue, int start, int end)
        {
            if (!_isPathValid)
            {
                MessageBox.Show("Folder check failed!\n The folder doesn´t exist or\n does not contain any .mp3 files.", "File renamer says...",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (start < 0 || end < start || end < 0)
            {
                MessageBox.Show("Char value must be positiv integer and \nend value cannot be less than start value.", "File renamer says...",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            int errors = 0;
            var fullPaths = Directory.EnumerateFiles(_folderURL, "*" + _fileType);
            var fullPaths1 = fullPaths as IList<string> ?? fullPaths.ToList();
            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*" + _fileType, SearchOption.TopDirectoryOnly)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();
            var cnt = enumerable.Count();

            pgb.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate()
            {
                pgb.Value = 0;
                pgb.Maximum = cnt;
            }));

            for (int i = 0; i < cnt; i++)
            {
                var newFileName = enumerable[i].Remove(start, end);
                newFileName = newFileName.Remove(newFileName.Length - _fileType.Length, _fileType.Length);
                newFileName += insertValue + _fileType;

                try
                {
                    var finalPath = _folderURL + "\\" + newFileName;
                    var paths = fullPaths as IList<string> ?? fullPaths1.ToList();
                    File.Move(paths.ElementAt(i), finalPath);
                }
                catch (IOException)
                {
                    File.Delete(enumerable[i]);
                    errors++;
                }

                increaseProgress(i);
            }
            SystemSounds.Beep.Play();

            if (errors > 0)
            {
                MessageBox.Show(errors + " duplicates removed.", "FileRenamer says...", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void CapitalizeAll()
        {
            if (!_isPathValid)
            {
                MessageBox.Show("Folder check failed!\n The folder doesn´t exist or\n does not contain any .mp3 files.", "File renamer says...",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            int errors = 0;
            var fullPaths = Directory.EnumerateFiles(_folderURL, "*" + _fileType);

            var fullPaths1 = fullPaths as IList<string> ?? fullPaths.ToList();
            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*" + _fileType)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();
            var cnt = enumerable.Count();

            pgb.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate()
            {
                pgb.Value = 0;
                pgb.Maximum = cnt;
            }));

            for (int i = 0; i < cnt; i++)
            {
                var newFileName = enumerable.ElementAt(i).Capitalize();
                var newPath = _folderURL + "\\" + newFileName;
                var paths = fullPaths as IList<string> ?? fullPaths1.ToList();
                try
                {
                    File.Move(paths.ElementAt(i), newPath);

                }
                catch (IOException)
                {
                    File.Delete(enumerable[i]);
                            errors++;
                }

                increaseProgress(i);
            }
            if (errors > 0)
            {
                MessageBox.Show(errors + " duplicates removed.", "FileRenamer says...", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #region Test Methods

        public string TestReplaceStrings(string removeValue, string replaceValue, out string oldName)
        {
            if (!_isPathValid)
            {
                MessageBox.Show("Folder check failed!\n The folder doesn´t exist or\n does not contain any .mp3 files.", "File renamer says...",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                oldName = null;
                return null;
            }

            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*" + _fileType, SearchOption.TopDirectoryOnly)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();

            Random rand = new Random();
            var randVal = rand.Next(0, enumerable.Count - 1);
            oldName = enumerable.ElementAt(randVal);

            var newFileName = enumerable[randVal].Replace(removeValue, replaceValue);

            return newFileName;
        }

        public string TestInsertAndRemove(string insertValue, int start, int end, out string oldName)
        {
            if (!_isPathValid)
            {
                MessageBox.Show("Folder check failed!\n The folder doesn´t exist or\n does not contain any .mp3 files.", "File renamer says...",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                oldName = null;
                return null;
            }

            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*" + _fileType, SearchOption.TopDirectoryOnly)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();

            Random rand = new Random();
            var randVal = rand.Next(0, enumerable.Count - 1);
            oldName = enumerable.ElementAt(randVal);

            var newFileName = enumerable[randVal].Remove(start, end);
            newFileName = newFileName.Remove(newFileName.Length - _fileType.Length, _fileType.Length);

            newFileName += insertValue + _fileType;
            return newFileName;
        }

        #endregion

        private void increaseProgress(int i)
        {
            pgb.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate ()
            {
                pgb.Value = i + 1;
            }));
        }
    }

    public static class Extension
    {
        public static string Capitalize(this string value)
        {
            var splittet = value.Split(' ');

            foreach (var part in splittet)
            {
                if (part == "")
                {
                    continue;
                }
                var split = part.ToCharArray();
                split[0] = char.ToUpper(split[0]);

                value = value.Replace(part, new string(split));
            }

            return value;
        }
    }
}

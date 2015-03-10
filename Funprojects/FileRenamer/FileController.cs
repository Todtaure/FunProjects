using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;

namespace FileRenamer
{
    public class FileController
    {
        private bool _isPathValid;
        private string _folderURL;

        public FileController()
        {
            _folderURL = null;
            _isPathValid = false;
        }

        public bool CheckFilePath(string path)
        {
            _isPathValid = false;
            if (Directory.Exists(path) && Directory.EnumerateFiles(path, "*.mp3").Any())
            {
                _folderURL = path;
                _isPathValid = true;
            }
            return _isPathValid;
        }

        public void ReplaceStrings(string removeValue, string replaceValue)
        {
            if (!_isPathValid)
            {
                MessageBox.Show("Folder check failed!\n The folder doesn´t exist or\n does not contain any .mp3 files.", "File renamer says...",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var fullPaths = Directory.EnumerateFiles(_folderURL, "*.mp3");

            var fullPaths1 = fullPaths as IList<string> ?? fullPaths.ToList();
            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*.mp3", SearchOption.TopDirectoryOnly)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();
            var cnt = enumerable.Count();
            for (int i = 0; i < cnt; i++)
            {
                var newFileName = enumerable.ElementAt(i).Replace(removeValue, replaceValue);
                var newPath = _folderURL + "\\" + newFileName;
                var paths = fullPaths as IList<string> ?? fullPaths1.ToList();
                File.Move(paths.ElementAt(i), newPath);
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
            var fullPaths = Directory.EnumerateFiles(_folderURL, "*.mp3");
            var fullPaths1 = fullPaths as IList<string> ?? fullPaths.ToList();
            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*.mp3", SearchOption.TopDirectoryOnly)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();
            var cnt = enumerable.Count();
            for (int i = 0; i < cnt; i++)
            {
                var newFileName = enumerable[i].Remove(start, end);
                newFileName = newFileName.Remove(newFileName.Length - 4, 4);
                newFileName += insertValue + ".mp3";

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
            var fullPaths = Directory.EnumerateFiles(_folderURL, "*.mp3");

            var fullPaths1 = fullPaths as IList<string> ?? fullPaths.ToList();
            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*.mp3")
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();
            var cnt = enumerable.Count();
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

            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*.mp3", SearchOption.TopDirectoryOnly)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();

            Random rand = new Random();
            oldName = enumerable.ElementAt(rand.Next(0, enumerable.Count - 1));

            var newFileName = enumerable.ElementAt(0).Replace(removeValue, replaceValue);

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

            var musicFiles = from fullMusicFiles in Directory.EnumerateFiles(_folderURL, "*.mp3", SearchOption.TopDirectoryOnly)
                             select Path.GetFileName(fullMusicFiles);
            var enumerable = musicFiles as IList<string> ?? musicFiles.ToList();

            Random rand = new Random();
            oldName = enumerable.ElementAt(rand.Next(0, enumerable.Count - 1));

            var newFileName = enumerable[0].Remove(start, end);
            newFileName = newFileName.Remove(newFileName.Length - 4, 4);

            newFileName += insertValue + ".mp3";
            return newFileName;
        }

        #endregion
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

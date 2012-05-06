using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace DvdCollection
{
    public class DvdReader
    {
        public List<MovieInfo> ReadDvd ()
        {
            EnsureDvdPathExists ();
            List<string> files = new List<string> ();
            try
            {
                files.AddRange (Directory.GetFiles (m_path, "*.avi", SearchOption.AllDirectories));
                files.AddRange (Directory.GetFiles (m_path, "*.mpg", SearchOption.AllDirectories));
            }
            catch
            {
                MessageBox.Show ("Lesen der DVD fehlgeschlagen.\n\nIst eine DVD eingelegt?", "Lesefehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            if (files.Count == 0)
            {
                MessageBox.Show ("DVD ist leer...", "Keine Daten", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            string dvdLocation = GetDvdLocationFromUser ();
            List<MovieInfo> result = new List<MovieInfo> ();
            foreach (string file in files)
            {
                string dbRelevantTitle;
                string movieTitle = GetTitleFromFileName (file, out dbRelevantTitle);
                result.Add (new MovieInfo ()
                {
                    Dvd = dvdLocation,
                    Title = movieTitle,
                    DbRelevantTitle = dbRelevantTitle
                });
            }

            return result;
        }

        private string GetDvdLocationFromUser ()
        {
            return "3/99";
        }

        private string GetTitleFromFileName (string filePath, out string dbRelevantTitle)
        {
            string fileName = Path.GetFileNameWithoutExtension (filePath);
            string[] folders = (filePath.Substring (m_path.Length)).Split (new char[] { '\\' },
                StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert (folders.GetLength (0) > 0, "No folders??!?");
            if (folders.GetLength (0) == 1)
            {
                dbRelevantTitle = fileName;
                return fileName;
            }

            dbRelevantTitle = folders[folders.GetLength (0) - 2];
            return dbRelevantTitle + "- " + fileName;
        }

        private void EnsureDvdPathExists ()
        {
            if (m_path == null)
            {
                //m_path = @"F:\";
                m_path = @"E:\___Videos zum Brennen";
            }
        }

        private string m_path;
    }
}

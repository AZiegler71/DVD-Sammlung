using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using DirectShowLib;
using System.Runtime.InteropServices;

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
                files.AddRange (Directory.GetFiles (m_path, "*.mpg", SearchOption.AllDirectories));
                files.AddRange (Directory.GetFiles (m_path, "*.avi", SearchOption.AllDirectories));
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
                MovieFileData fileData = GetMovieFileData (file);
                result.Add (new MovieInfo (movieTitle, dvdLocation, fileData)
                {
                    DbRelevantTitle = dbRelevantTitle
                });
            }

            return result;
        }

        private MovieFileData GetMovieFileData (string file)
        {
            try
            {
                Debug.WriteLine (string.Format ("Getting info for movie \"{0}\"", file));

                FilterGraph graphbuilder = new FilterGraph ();
                ((IGraphBuilder) graphbuilder).RenderFile (file, null);

                int x;
                int y;
                long duration;
                ((IBasicVideo) graphbuilder).GetVideoSize (out x, out y);
                ((IMediaSeeking) graphbuilder).GetDuration (out duration);

                Marshal.ReleaseComObject (graphbuilder);

                return new MovieFileData ()
                {
                    X = x,
                    Y = y,
                    Duration = (double) duration / 6e8d
                };
            }
            catch
            {
                return new MovieFileData () { Duration = double.NaN, X = -1, Y = -1 };
            }
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

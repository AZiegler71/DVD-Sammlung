using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using DirectShowLib;

namespace DvdCollection
{
    public class DvdReader
    {
        public DvdReader ()
        {
            AskDvdPath ();
        }

        public List<MovieInfo> ReadDvd ()
        {
            bool aborted;
            string dvdLocation = GetDvdNameFromUser (out aborted);
            if (aborted)
            {
                return null;
            }

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

            List<MovieInfo> result = new List<MovieInfo> ();
            foreach (string file in files)
            {
                MovieFileData fileData = GetMovieFileData (file);
                result.Add (new MovieInfo (file.Substring (m_path.Length), dvdLocation, fileData));
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

        private string GetDvdNameFromUser (out bool aborted)
        {
            aborted = true;
            AskMovieLocationDialog dialog = new AskMovieLocationDialog ();
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog ();
            if (dialog.DialogResult == true)
            {
                aborted = false;
                return dialog.DvdName;
            }

            return null;
        }

        private void AskDvdPath ()
        {
            bool canceled;
            string result = Utils.GetFolderFromUser ("DVD-Stammverzeichnis auswählen", out canceled);
            if (!canceled)
            {
                m_path = result;
            }
            else
            {
                MessageBox.Show ("Applikation wird geschlossen");
                Application.Current.MainWindow.Close ();
            }
        }

        private string m_path;
    }
}

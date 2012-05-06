using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using DirectShowLib;
using DvdCollection.Properties;

namespace DvdCollection
{
    public class DvdReader
    {
        /// <summary>
        /// Returns null if failed.
        /// </summary>
        /// <param name="closeAppWhenFailed">If true starts to close the application when no path was selected by the user.</param>
        public List<MovieInfo> ReadDvd (bool closeAppWhenFailed)
        {
            if (!EnsureDvdPathExists ())
            {
                if (closeAppWhenFailed)
                {
                    MessageBox.Show ("Applikation wird geschlossen");
                    Application.Current.MainWindow.Close ();
                    return null;
                }
                else
                {
                    return null;
                }
            }

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
            SimpleTextInputDialog dialog = new SimpleTextInputDialog ("Ort", "DVD-Name:", Settings.Default.LastLocation);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog ();
            if (dialog.DialogResult == true)
            {
                aborted = false;
                Settings.Default.LastLocation = dialog.Text;
                return dialog.Text;
            }

            return null;
        }

        private bool EnsureDvdPathExists ()
        {
            if (m_path != null)
            {
                return true;
            }

            bool canceled;
            string result = Utils.GetFolderFromUser ("DVD-Stammverzeichnis auswählen", out canceled);
            if (!canceled)
            {
                m_path = result;
                return true;
            }

            return false;
        }

        private string m_path;
    }
}

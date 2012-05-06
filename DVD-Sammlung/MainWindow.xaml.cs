using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DvdCollection.PersistentList;
using DvdCollection.Properties;

namespace DvdCollection
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private MovieList m_movies;
        public MovieList Movies
        {
            get { return m_movies; }
            private set
            {
                m_movies = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Movies"));
                }
            }
        }

        public MainWindow ()
        {
            Movies = new MovieList ();
            Movies.LoadAll ();

            InitializeComponent ();

            Loaded += new RoutedEventHandler (MainWindowLoaded);
        }

        protected override void OnClosing (CancelEventArgs e)
        {
            base.OnClosing (e);
            Settings.Default.Save ();
        }

        private void MainWindowLoaded (object sender, RoutedEventArgs e)
        {
            m_dvdReader = new DvdReader ();
        }

        private void AddDvdClick (object sender, RoutedEventArgs args)
        {
            List<MovieInfo> newEntries = m_dvdReader.ReadDvd (true);
            if (newEntries == null)
            {
                return;
            }

            foreach (MovieInfo info in newEntries)
            {
                // Check for exact doubles
                MovieInfo existingInfo = (from x in Movies
                                          where x.Title == info.Title
                                          select x).FirstOrDefault ();
                if (existingInfo != null)
                {
                    string existingMovieInfo = BuildInfoString (new List<MovieInfo> () { existingInfo });
                    string newMovieInfo = BuildInfoString (new List<MovieInfo> () { info });
                    if (MessageBox.Show (string.Format ("Es gibt schon einen Eintrag des Films\n{0}\n\nVorhandener Eintrag:\n{1}\n\nSoll der vorhandene Eintrag überschrieben werden?",
                        newMovieInfo, existingMovieInfo),
                        "Überschreiben?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        continue;
                    }
                    Movies.Remove (existingInfo);
                }
                else
                {
                    // Inform in case of potential doubles
                    List<MovieInfo> maybeExistingInfos = (from x in Movies
                                                          where x.Title.Contains (info.Title) || info.Title.Contains (x.Title)
                                                          select x).ToList ();
                    if (maybeExistingInfos.Count > 0)
                    {
                        string existingMovieInfos = BuildInfoString (maybeExistingInfos);
                        string newMovieInfo = BuildInfoString (new List<MovieInfo> () { info });
                        MessageBox.Show (string.Format ("Eventuell gibt es schon einen oder mehrere Einträge des Films\n{0}\n\nPotentielle Kandidaten:\n{1}\n\nLöschen Sie bitte eigenhändig eventuelle Einträge!",
                            newMovieInfo, existingMovieInfos));
                    }
                }

                Movies.Add (info);
            }
        }

        private string BuildInfoString (List<MovieInfo> infos)
        {
            List<string> result = new List<string> ();
            foreach (MovieInfo info in infos)
            {
                result.Add (string.Format ("Titel: \"{0}\"\nBildgröße: {1}\nDauer: {2} Minuten",
                    info.Title,
                    info.Size,
                    info.Duration));
            }

            return string.Join ("\n\n", result.ToArray ());
        }

        private void CompleteFromDatabase (object sender, RoutedEventArgs args)
        {
        }

        private void CompareDbWithFolder (object sender, RoutedEventArgs args)
        {
        }

        private void SearchTextChanged (object sender, TextChangedEventArgs args)
        {
            string text = (sender as TextBox).Text;
            m_movies.ApplyFilter (text);
        }

        private DvdReader m_dvdReader;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

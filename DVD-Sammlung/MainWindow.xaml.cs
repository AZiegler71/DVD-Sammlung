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
            List<MovieInfo> newEntries = m_dvdReader.ReadDvd ();
            if (newEntries == null)
            {
                return;
            }

            foreach (MovieInfo info in newEntries)
            {
                MovieInfo existingInfo = (from x in Movies
                                          where x.Title == info.Title
                                          select x).FirstOrDefault ();
                if (existingInfo != null)
                {
                    if (MessageBox.Show (string.Format ("Es gibt schon einen Eintrag des Films \"{0}\" (DVD {1}).\nBildgröße: {2}\nDauer: {3} Minuten\n\nSoll der Eintrag überschrieben werden?\nÜberschreiben mit:\nBildgröße: {4}\nDauer: {5} Minuten",
                        existingInfo.Title, existingInfo.DvdName,
                        existingInfo.Size, existingInfo.Duration,
                        info.Size, info.Duration),
                        "Überschreiben?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        continue;
                    }
                    Movies.Remove (existingInfo);
                }

                Movies.Add (info);
            }
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

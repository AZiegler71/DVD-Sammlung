using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;

namespace DvdCollection
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<MovieInfo> m_movieList;
        public ObservableCollection<MovieInfo> MovieList
        {
            get { return m_movieList; }
            private set
            {
                m_movieList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("MovieList"));
                }
            }
        }

        public MainWindow ()
        {
            InitializeComponent ();

            List<MovieInfo> movies = MoviePersistence.LoadMovies ();
            MovieList = new ObservableCollection<MovieInfo> (movies);

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
                MovieInfo existingInfo = (from x in MovieList
                                          where x.Title == info.Title
                                          select x).FirstOrDefault ();
                if (existingInfo != null)
                {
                    if (MessageBox.Show ("Es gibt schon einen Eintrag des Films \"{0}\" (DVD {1}).\n\nSoll der Eintrag überschrieben werden?",
                        "Überschreiben?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        continue;
                    }
                    MovieList.Remove (existingInfo);
                }

                MovieList.Add (info);
            }
        }

        private void CompleteFromDatabase (object sender, RoutedEventArgs args)
        {

        }

        private DvdReader m_dvdReader;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

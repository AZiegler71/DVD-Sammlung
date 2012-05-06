using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DvdCollection.InfoRequest
{
    public partial class SelectSearchResultDialog : Window, INotifyPropertyChanged
    {
        internal MoviePage SelectedMoviePage { get; private set; }

        private ObservableCollection<MoviePageVM> m_searchResults;
        public ObservableCollection<MoviePageVM> SearchResults
        {
            get { return m_searchResults; }
            private set
            {
                m_searchResults = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("SearchResults"));
                }
            }
        }

        private string m_movieTitle;
        public string MovieTitle
        {
            get { return m_movieTitle; }
            private set
            {
                m_movieTitle = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("MovieName"));
                }
            }
        }

        public SelectSearchResultDialog (string movieTitle, IList<MoviePage> candidates)
        {
            SearchResults = new ObservableCollection<MoviePageVM> (candidates.Select (x => new MoviePageVM (x)));
            MovieTitle = movieTitle;

            InitializeComponent ();
        }

        private void MouseDoubleClick (object sender, MouseButtonEventArgs args)
        {
            MoviePageVM selected = gridView.SelectedItem as MoviePageVM;
            if (selected == null)
                return;

            SelectedMoviePage = selected.SourceDataModel;
            Close ();
        }

        private void GridViewSelectionChanged (object sender, SelectionChangedEventArgs args)
        {
            okButton.IsEnabled = true;
        }

        private void OkOnClick (object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close ();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

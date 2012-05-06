using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

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
            MovieList = new ObservableCollection<MovieInfo> ();

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

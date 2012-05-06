using System.ComponentModel;

namespace DvdCollection
{
    public class MovieInfo : INotifyPropertyChanged
    {
        private string m_title;
        public string Title
        {
            get { return m_title; }
            set
            {
                m_title = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Title"));
                }
            }
        }

        private string m_dvd;
        public string Dvd
        {
            get { return m_dvd; }
            set
            {
                m_dvd = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Dvd"));
                }
            }
        }

        private string m_genre;
        public string Genre
        {
            get { return m_genre; }
            set
            {
                m_genre = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Genre"));
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

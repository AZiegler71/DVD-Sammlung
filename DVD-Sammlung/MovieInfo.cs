using System.ComponentModel;
using System.Windows.Controls;

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

        public string DbRelevantTitle { get; set; }

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

        private string m_genres;
        public string Genres
        {
            get { return m_genres; }
            set
            {
                m_genres = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Genres"));
                }
            }
        }

        private string m_coverPath;
        public string CoverPath
        {
            get { return m_coverPath; }
            set
            {
                m_coverPath = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("CoverPath"));
                }
            }
        }

        private string m_description;
        public string Description
        {
            get { return m_description; }
            set
            {
                m_description = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Description"));
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

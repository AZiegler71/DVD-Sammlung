using System.ComponentModel;
using System.Windows.Controls;
using System;

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

        private string m_rating;
        public string Rating
        {
            get { return m_rating; }
            set
            {
                m_rating = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Rating"));
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

        public string Resolution
        {
            get { return string.Format ("{0}x{1}", FileData.X.ToString (), FileData.Y.ToString ()); }
        }

        public int Duration
        {
            get { return (int) Math.Round (FileData.Duration); }
        }

        private MovieFileData m_fileData;
        public MovieFileData FileData
        {
            get { return m_fileData; }
            private set
            {
                m_fileData = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Resolution"));
                    PropertyChanged (this, new PropertyChangedEventArgs ("Duration"));
                }
            }
        }

        public string DbRelevantTitle { get; set; }

        public MovieInfo (string title, string dvd, MovieFileData fileData)
        {
            Title = title;
            Dvd = dvd;
            FileData = fileData;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

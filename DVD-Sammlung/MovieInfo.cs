using System.ComponentModel;
using System.Windows.Controls;
using System;
using System.Diagnostics;
using System.IO;

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

        public string RawTitlePath { get; set; }

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

        private string m_dvdName;
        public string DvdName
        {
            get { return m_dvdName; }
            set
            {
                m_dvdName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("DvdName"));
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

        public MovieInfo (string title, string dvdName, MovieFileData fileData)
        {
            Title = title;
            DvdName = dvdName;
            FileData = fileData;
        }

        public string GetDbRelevantTitle ()
        {
            string[] folders = RawTitlePath.Split (new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert (folders.GetLength (0) > 0, "No folders??!?");
            if (folders.GetLength (0) == 1)
            {
                return Title;
            }

            return folders[folders.GetLength (0) - 2];
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

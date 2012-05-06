using System.ComponentModel;
using System.Windows.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using System.Text;

namespace DvdCollection
{
    public class MovieInfo : INotifyPropertyChanged
    {
        private string m_title;
        public string Title
        {
            get
            {
                if (m_title == null)
                {
                    m_title = BuildTitle ();
                }
                return m_title;
            }
        }

        private string m_rawTitlePath;
        public string RawTitlePath
        {
            get { return m_rawTitlePath; }
            set
            {
                m_rawTitlePath = value;
                m_title = null;
            }
        }

        private int? m_year;
        public int? Year
        {
            get { return m_year; }
            set
            {
                m_year = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("Year"));
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

        private BitmapSource m_coverImage;
        public BitmapSource CoverImage
        {
            get { return m_coverImage; }
            set
            {
                m_coverImage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged (this, new PropertyChangedEventArgs ("CoverImage"));
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

        public string Size
        {
            get { return string.Format ("{0}x{1}", FileData.X.ToString (), FileData.Y.ToString ()); }
        }

        public int Resolution
        {
            get { return FileData.X * FileData.Y; }
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
                    PropertyChanged (this, new PropertyChangedEventArgs ("Size"));
                    PropertyChanged (this, new PropertyChangedEventArgs ("Resolution"));
                    PropertyChanged (this, new PropertyChangedEventArgs ("Duration"));
                }
            }
        }

        public MovieInfo (string rawTitlePath, string dvdName, MovieFileData fileData)
        {
            RawTitlePath = rawTitlePath;
            DvdName = dvdName;
            FileData = fileData;
        }

        public string GetDbRelevantTitle ()
        {
            string[] folders = RawTitlePath.Split (new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert (folders.GetLength (0) > 0, "No folders??!?");
            return folders[0];
        }

        private string BuildTitle ()
        {
            string fileName = Path.GetFileNameWithoutExtension (RawTitlePath);
            string[] folders = RawTitlePath.Split (new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert (folders.GetLength (0) > 0, "No folders??!?");
            if (folders.GetLength (0) == 1)
            {
                return fileName;
            }
            else
            {
                StringBuilder builder = new StringBuilder ();
                for (int i = folders.GetLength (0); i > 0; i--)
                {
                    if (i > 1)
                    {
                        builder.Append (folders[folders.GetLength (0) - i]);
                        builder.Append ("- ");
                    }
                    else
                    {
                        builder.Append (fileName);
                    }
                }
                return builder.ToString ();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

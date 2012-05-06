using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DvdCollection.PersistentList
{
    public class MovieList : ObservableCollection<MovieInfo>
    {
        public void LoadAll ()
        {
            List<MovieInfo> movies = MoviePersistence.LoadMovies ();
            foreach (MovieInfo info in movies)
            {
                m_fullList.Add (info);
            }

            ApplyFilter (string.Empty);
        }

        public new bool Remove (MovieInfo movieInfo)
        {
            MoviePersistence.Delete (movieInfo);

            base.Remove (movieInfo);
            return m_fullList.Remove (movieInfo);
        }

        public new void Add (MovieInfo movieInfo)
        {
            MoviePersistence.Add (movieInfo);
            base.Add (movieInfo);
        }

        public void ApplyFilter (string filterText)
        {
            m_filterText = filterText == null ? string.Empty : filterText.ToLower ();

            base.Clear ();
            foreach (MovieInfo info in m_fullList)
            {
                if (m_filterText == string.Empty || FilterApplies (info))
                {
                    base.Add (info);
                }
            }
        }

        private bool FilterApplies (MovieInfo movieInfo)
        {
            return movieInfo.Title.ToLower ().Contains (m_filterText);
        }

        private string m_filterText;
        private List<MovieInfo> m_fullList = new List<MovieInfo> ();
    }
}
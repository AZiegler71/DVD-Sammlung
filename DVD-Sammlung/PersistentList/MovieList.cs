using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
            string newFilterText = filterText == null ? string.Empty : filterText.ToLower ();

            // Try to speed up by reducing the number of items to search for
            bool useFilteredListAsBase = string.IsNullOrEmpty (m_filterText) ? false : newFilterText.StartsWith (m_filterText);
            m_filterText = newFilterText;

            List<MovieInfo> filterBaseList;
            if (useFilteredListAsBase)
            {
                filterBaseList = new List<MovieInfo> ();
                foreach (MovieInfo info in this)
                {
                    filterBaseList.Add (info);
                }
            }
            else
            {
                filterBaseList = m_fullList;
            }

            base.Clear ();
            foreach (MovieInfo info in filterBaseList)
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
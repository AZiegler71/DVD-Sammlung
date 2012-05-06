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
                this.Add (info);
            }
        }

        public new bool Remove (MovieInfo movieInfo)
        {
            MoviePersistence.Delete (movieInfo);
            return base.Remove (movieInfo);
        }

        public new void Add (MovieInfo movieInfo)
        {
            MoviePersistence.Add (movieInfo);
            base.Add (movieInfo);
        }
    }
}
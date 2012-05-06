using System.Collections.Generic;

namespace DvdCollection
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public static class MoviePersistence
    {
        public static void StoreMovies (List<MovieInfo> movieList)
        {
        }

        public static List<MovieInfo> LoadMovies ()
        {
            return new List<MovieInfo> () { 
                new MovieInfo ()
                { 
                    Title = "titel", Dvd = "2/01", Genres = "Ulk", CoverPath=@"D:\Eigene Dateien Alex\Visual Studio Projects\DVD-Sammlung\DVD-Sammlung\bin\Debug\CoverImages\yfggfffgfhfhysfds.jpg", Description="Luschtiger Film!"
                },
                new MovieInfo ()
                { 
                    Title = "blabla", Dvd = "2/03", Genres = "Horror", Description="Bäüsefilm"
                }};
        }
    }
}
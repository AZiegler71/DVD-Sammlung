
namespace DvdCollection.InfoRequest
{
    class MoviePage
    {
        public string MovieName { get; private set; }

        public string OriginalMovieName { get; private set; }

        public string RelativeLink { get; private set; }

        public int Year { get; private set; }

        public DegreeOfMatch Match { get; private set; }

        public MoviePage (string movieName, string originalMovieName, string relativeLink, int year, DegreeOfMatch match)
        {
            MovieName = movieName;
            OriginalMovieName = originalMovieName;
            RelativeLink = relativeLink;
            Year = year;
            Match = match;
        }
    }
}

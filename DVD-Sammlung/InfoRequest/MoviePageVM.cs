using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DvdCollection.InfoRequest
{
    public class MoviePageVM
    {
        public MoviePage SourceDataModel { get; private set; }

        public string MovieName { get; private set; }

        public string OriginalMovieName { get; private set; }

        public string Year { get; private set; }

        public string Match { get; private set; }

        public MoviePageVM (MoviePage candidate)
        {
            SourceDataModel = candidate;

            MovieName = candidate.MovieName;
            OriginalMovieName = candidate.OriginalMovieName;
            Year = candidate.Year.ToString ();
            Match = DegreeOfMatchText.GetText (candidate.Match);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace DvdCollection.InfoRequest
{
    class OfdbExtractor
    {
        internal IList<MoviePage> ExtractSearchResults (string htmlPage)
        {
            IList<MoviePage> result = new List<MoviePage> ();
            IList<MatchDegrees> matchDegrees = FindMatchDegrees (htmlPage)
                .OrderByDescending (x => x.Index)
                .ToList ();

            Regex regex = new Regex (
                "[0-9]+\\. <a href=\"([^\"]+)[^>]+>[^>]+>([^<]+)<font size=\"1\"> / ([^<^>]+)</font> \\(([0-9]+)\\)</a><br>");
            MatchCollection matches = regex.Matches (htmlPage);
            foreach (Match match in matches)
            {
                string movieName = match.Groups[2].Value;
                string originalMovieName = match.Groups[3].Value;
                string relativeLink = match.Groups[1].Value;
                string yearAsString = match.Groups[4].Value;
                DegreeOfMatch degree = matchDegrees.First (x => x.Index < match.Index).Degree;

                int year;
                if (!int.TryParse (yearAsString, out year))
                    throw new Exception ("Year could not be parsed as string: \"" + yearAsString + "\"");

                MoviePage newMovie = new MoviePage (movieName, originalMovieName, relativeLink, year, degree);
                result.Add (newMovie);
            }

            return result;
        }

        private IList<MatchDegrees> FindMatchDegrees (string htmlPage)
        {
            IList<MatchDegrees> result = new List<MatchDegrees> ();
            foreach (DegreeOfMatch degree in Enum.GetValues (typeof (DegreeOfMatch)))
            {
                int index = htmlPage.IndexOf (DegreeOfMatchHeaders.GetHeader (degree));
                if (index >= 0)
                    result.Add (new MatchDegrees (index, degree));
            }
            return result;
        }

        private class MatchDegrees
        {
            public int Index { get; private set; }

            public DegreeOfMatch Degree { get; private set; }

            public MatchDegrees (int index, DegreeOfMatch degree)
            {
                Index = index;
                Degree = degree;
            }
        }
    }
}

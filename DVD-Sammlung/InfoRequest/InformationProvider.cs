using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows;

namespace DvdCollection.InfoRequest
{
    class InformationProvider
    {
        public void CompleteFromDatabase (MovieInfo movieInfo)
        {
            OfdbExtractor extractor = new OfdbExtractor ();
            bool tryWithCustomTitle;
            IList<MoviePage> searchResults;
            string searchTitle = movieInfo.Title;
            do
            {
                tryWithCustomTitle = false;

                string searchResultPage = GetSearchResultPage (searchTitle);
                searchResults = extractor.ExtractSearchResults (searchResultPage);
                if (searchResults.Count == 0)
                {
                    SimpleTextInputDialog dialog = new SimpleTextInputDialog ("Kein Fund",
                        "Alternativer Titel für Suche:", searchTitle);
                    dialog.Owner = Application.Current.MainWindow;
                    dialog.ShowDialog ();
                    if (dialog.DialogResult == true)
                    {
                        searchTitle = dialog.Text;
                        tryWithCustomTitle = true;
                    }
                    else
                    {
                        return;
                    }
                }
            } while (tryWithCustomTitle);

            MoviePage selectedResult = UserSelectSearchResult (searchTitle, searchResults.OrderBy (x => x.Match).ToList ());
            if (selectedResult == null)
                return;


        }

        private string GetSearchResultPage (string title)
        {
            return DownloadPage (DB_HTTP_ROOT + "view.php?page=suchergebnis&SText=" + Utils.HttpPostMask (title));
        }

        private string DownloadPage (string address)
        {
            string result;
            using (WebClient client = new WebClient ())
            {
                client.Encoding = ASCIIEncoding.UTF8;
                result = client.DownloadString (address);
            }
            return result;
        }

        private MoviePage UserSelectSearchResult (string movieTitle, IList<MoviePage> candidates)
        {
            // if there is exactly one perfectly matching candidate, return that silently
            var perfectCandidates = candidates.Where (x => x.Match == DegreeOfMatch.Exact).ToList ();
            if (perfectCandidates.Count == 1)
                return perfectCandidates[0];

            SelectSearchResultDialog dialog = new SelectSearchResultDialog (movieTitle, candidates);
            dialog.ShowDialog ();
            return dialog.SelectedMoviePage;
        }

        private static readonly string DB_HTTP_ROOT = "http://www.ofdb.de/";

        public bool tryWithOtherTitle { get; set; }

        public bool tryWithCustomTitle { get; set; }
    }
}

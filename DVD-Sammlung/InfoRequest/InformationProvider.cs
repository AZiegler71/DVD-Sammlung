using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DvdCollection.InfoRequest
{
    class InformationProvider
    {
        public void CompleteFromDatabase (MovieInfo movieInfo)
        {
            OfdbExtractor extractor = new OfdbExtractor ();
            string searchResultPage = GetSearchResultPage ("Doom");

            IList<MoviePage> results = extractor.ExtractSearchResults (searchResultPage);
        }

        private string GetSearchResultPage (string title)
        {
            string result;
            using (WebClient client = new WebClient ())
            {
                result = client.DownloadString ("http://www.ofdb.de/view.php?page=suchergebnis&SText=" + title);
            }
            return result;
        }
    }
}

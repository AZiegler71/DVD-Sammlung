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
        }

        private string GetSearchResultPage (string title)
        {
            long contentLength;

            WebPostRequest webRequest = new WebPostRequest ("http://www.ofdb.de/view.php?page=suchergebnis&SText=" + title);
            string result = webRequest.GetResponse ();



            //string result;
            //UTF8Encoding textConverterUTF8 = new UTF8Encoding ();
            //byte[] post = textConverterUTF8.GetBytes ("?page=suchergebnis&SText=" + title);

            //using (Stream stream = webRequest.PerformWebRequest ("http://www.ofdb.de/view.php", post, null,
            //    out contentLength, 5000))
            //{
            //    StreamReader reader = new StreamReader (stream);
            //    result = reader.ReadToEnd ();
            //}



            return result;
        }
    }
}

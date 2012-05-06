using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DvdCollection.InfoRequest
{
    static class Utils
    {
        public static string DecodeHtml (string html)
        {
            string result = HttpUtility.HtmlDecode (html);
            return result;
        }

        public static string HttpPostMask (string arg)
        {
            string result = HttpUtility.UrlPathEncode (arg);
            return result;
        }
    }
}

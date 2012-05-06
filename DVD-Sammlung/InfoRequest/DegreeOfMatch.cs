using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DvdCollection.InfoRequest
{
    public enum DegreeOfMatch
    {
        Exact = 0,
        NearlyExact = 1,
        Possible = 2
    }


    static class DegreeOfMatchText
    {
        public static string GetWebPageHeader (DegreeOfMatch degree)
        {
            switch (degree)
            {
                case DegreeOfMatch.Exact:
                    return "Exakte Ergebnisse";
                case DegreeOfMatch.NearlyExact:
                    return "Sehr &auml;hnliche Ergebnisse";
                case DegreeOfMatch.Possible:
                    return "Ungef&auml;hre Ergebnisse";
                default:
                    throw new NotImplementedException ();
            }
        }

        public static string GetText (DegreeOfMatch degree)
        {
            switch (degree)
            {
                case DegreeOfMatch.Exact:
                    return "Exakt";
                case DegreeOfMatch.NearlyExact:
                    return "Sehr ähnlich";
                case DegreeOfMatch.Possible:
                    return "Ungefähr";
                default:
                    throw new NotImplementedException ();
            }
        }
    }
}

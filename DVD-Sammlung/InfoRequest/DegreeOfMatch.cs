using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DvdCollection.InfoRequest
{
    enum DegreeOfMatch
    {
        Exact,
        NearlyExact,
        Possible
    }


    static class DegreeOfMatchHeaders
    {
        public static string GetHeader(DegreeOfMatch degree)
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
    }
}

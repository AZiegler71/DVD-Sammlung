using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace DvdCollection.InfoRequest
{
    class WebPostRequest
    {
        public WebPostRequest (string url)
        {
            m_request = WebRequest.Create (url);
            m_request.Method = "POST";
            m_queryData = new List<string> ();
        }

        public void Add (string key, string value)
        {
            m_queryData.Add (string.Format ("{0}={1}", key, HttpUtility.UrlEncode (value)));
        }

        public string GetResponse ()
        {
            // Set the encoding type
            m_request.ContentType = "application/x-www-form-urlencoded";

            // Build a string containing all the parameters
            string Parameters = String.Join ("&", m_queryData.ToArray ());
            m_request.ContentLength = Parameters.Length;

            // We write the parameters into the request
            StreamWriter sw = new StreamWriter (m_request.GetRequestStream ());
            sw.Write (Parameters);
            sw.Close ();

            // Execute the query
            HttpWebResponse response = (HttpWebResponse) m_request.GetResponse ();
            StreamReader sr = new StreamReader (response.GetResponseStream ());
            return sr.ReadToEnd ();
        }

        private WebRequest m_request;
        private List<string> m_queryData;





        /// <summary>
        /// Makes a request to URI and returns the response stream. Don´t forget to Close() when done! 
        /// Throws user exceptions but returns null if the user rejected the web-connection.
        /// </summary>
        /// <param name="uri">URI to send the request to.</param>
        /// <param name="postedBinary">Binary to send to the uri.</param>
        /// <param name="tag">String that is transmitted to the server in the request header ("$ENV{'HTTP_TAG'}"). 
        /// May be "".</param>
        /// <param name="contentLength">Returns the length of the stream content if available. If it is not
        /// available, -1 is returned.</param>
        /// <param name="timeOutDuration">Timeout to wait for the response in milliseconds.</param>
        /// <param name="preventAskUserConnect">If true the user is not asked if a web connection may be established 
        /// even if this is checked in options. Used e.g. when the user already knows that he wants to establish a 
        /// web connection.</param>
        /// <param name="worker">Progress bar to show the upload progress. May be null.</param>
        public Stream PerformWebRequest (string uri, byte[] postedBinary, string tag, out long contentLength, int timeOutDuration)
        {
            int packetSize = 1024; // Maximal packet size which is used for RequestStream (upload), to enable the ProgressBar
            contentLength = -1;

            HttpWebRequest myRequest = (HttpWebRequest) WebRequest.Create (uri);
            myRequest.ContentType = "application/octet-stream";
            myRequest.Timeout = timeOutDuration;
            if (postedBinary != null)
            {
                myRequest.ContentLength = postedBinary.Length;
                myRequest.Method = "POST";
            }

            if (tag != "")
            {
                myRequest.Headers.Add ("TAG", tag); // Is received using the command "$ENV{'HTTP_TAG'}" on the server
            }

            // Get web response
            WebResponse webResponse = null;
            Stream result = null;
            Stream requestStream = null;
            try
            {
                if (postedBinary != null)
                {
                    int fullPacketNumber = postedBinary.GetLength (0) / packetSize; // Number of packets that have to be sent in full size

                    requestStream = myRequest.GetRequestStream ();

                    // Write packets
                    int packetsSent = 0;
                    int bytesWritten = 0;
                    do
                    {
                        if (packetsSent < fullPacketNumber)
                        {
                            requestStream.Write (postedBinary, bytesWritten, packetSize);
                            bytesWritten += packetSize;
                        }
                        else // look for the rest that does not fit into a full packet
                        {
                            if (bytesWritten < postedBinary.GetLength (0))
                            {
                                requestStream.Write (postedBinary, bytesWritten, postedBinary.GetLength (0) - bytesWritten);
                            }

                            bytesWritten = postedBinary.GetLength (0);
                        }

                        packetsSent++;
                    } while (bytesWritten < postedBinary.GetLength (0));
                }

                // Return the response
                webResponse = myRequest.GetResponse ();
            }
            catch (Exception ex)
            {
                throw new Exception ("ErrorConnectToServer" + ex.ToString ());
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close ();
                }
            }

            // Code to use the WebResponse
            try
            {
                result = webResponse.GetResponseStream ();
                contentLength = webResponse.ContentLength;
            }
            catch
            {
                //CloseWebRequest ();
                throw;
            }

            return result;
        }
    }
}

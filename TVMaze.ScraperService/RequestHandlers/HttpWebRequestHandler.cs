using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TVMaze.ScraperService.Constrants;

namespace TVMaze.ScraperService.RequestHandlers
{
    public class HttpWebRequestHandler : IRequestHandler
    {
        public string GetSourceTvMazeContent(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.UserAgent = RequestConstants.UserAgentValue;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var content = string.Empty;

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw ex;                
            }

            return content;
        }
    }
}

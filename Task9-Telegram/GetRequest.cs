using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Task9_Telegram
{
    public class GetRequest
    {
        private HttpWebRequest _request;
        private string _url;
        public string Response { get; private set; }

        public GetRequest() { }
        public GetRequest(string path)
        {
            _url = path;

        }
        public void Run()
        {
            try
            {
                _request = (HttpWebRequest)WebRequest.Create(_url);
                _request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
                var result = response.GetResponseStream();
                if (result != null)
                {
                    Response = new StreamReader(result).ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}

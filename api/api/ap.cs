using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace api
{
    class ap
    {
        public List<Songs> audiolist;
        public void parse(string str)
        {
            while (!auth_property.Default.security)
            {
                Thread.Sleep(500);
                string url = "https://api.vk.com/method/audio.search?q=" + str + "&access_token=" + auth_property.Default.token;
                WebRequest request = WebRequest.Create(url);

                try
                {
                    WebResponse resp = request.GetResponse();
                    Stream stream = resp.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string js_a = reader.ReadToEnd();
                    reader.Close();
                    resp.Close();
                    js_a = HttpUtility.HtmlDecode(js_a);

                    JToken token = JToken.Parse(js_a);
                    audiolist = token["response"].Children().Skip(1).Select(c => c.ToObject<Songs>()).ToList();
                }
                catch { }
            }
        }
    }
}

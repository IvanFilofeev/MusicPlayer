using AxWMPLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace api
{
    public class SearchEngine
    {
        string _WhatToSearch;
        public List<Songs> _audiolist;        
       List<string> _urls = new List<string>();

        List<string> _searchResults = new List<string>();
        
       

        public SearchEngine(string WhatToSearch)
        {
            _WhatToSearch = WhatToSearch;
        }

        public void Search()
        {
            string url = "https://api.vk.com/method/audio.search?q=" + _WhatToSearch + "&access_token=" + auth_property.Default.token;
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
                _audiolist = token["response"].Children().Skip(1).Select(c => c.ToObject<Songs>()).ToList();

                


                for (int i = 0; i < _audiolist.Count(); i++)
                {
                  
                    _searchResults.Add(_audiolist[i].artist + " - " + _audiolist[i].title );
                    _urls.Add( _audiolist[i].url);
                }
                
            }
            catch { }
        }      


        public List<string> GetFiles()
        {
            return _searchResults;
        }
        public List<string> GetUrls()
        {
            return _urls;
        }

    }
}

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
    public class MyMusicEngine
    {      

        public List<Songs> _audiolistMyMusic;
        List<string> _MyMusicPlayList = new List<string>();
        List<string> _MyMusicurls = new List<string>();


        public void MyMusicLoad()
        {            
            WebRequest request =WebRequest.Create("https://api.vk.com/method/audio.get?owner_id" + auth_property.Default.id + "&need_user=0&access_token=" + auth_property.Default.token);

            try
            {
                WebResponse responce = request.GetResponse();
                Stream stream = responce.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string js_a = reader.ReadToEnd();
                reader.Close();
                responce.Close();
                js_a = HttpUtility.HtmlDecode(js_a);

                JToken token = JToken.Parse(js_a);
                _audiolistMyMusic = token["response"].Children().Skip(1).Select(c => c.ToObject<Songs>()).ToList();


                for (int i = 0; i < _audiolistMyMusic.Count(); i++)
                {

                    _MyMusicPlayList.Add(_audiolistMyMusic[i].artist + " - " + _audiolistMyMusic[i].title);
                    _MyMusicurls.Add(_audiolistMyMusic[i].url);

                }

            }
            catch { }           


        }

        public List<string> MyMusicLoadGetFiles()
        {

            return _MyMusicPlayList;
        }
        public List<string> MyMusicGetUrls()
        {
            return _MyMusicurls;
        }
    }
}



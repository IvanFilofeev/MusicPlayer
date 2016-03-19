using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.DirectX.AudioVideoPlayback;
using System.Xml.Linq;

namespace mpDataBase
{
    public interface PashaAudio
    {
        string url { get; }
        string artist { get; }
        string title { get; }
        string aid { get; }
    }
    public class mpLocalAudio : mpAudio
    {
        internal mpLocalAudio(PashaAudio vkAudio)
        {
            Link = mpDataBaseController.Instance().generateFileName();
            HttpWebResponse response =
              (HttpWebResponse)((HttpWebRequest)WebRequest.Create(vkAudio.url)).GetResponse();
            FileStream fs = new FileStream(Link, FileMode.Create);
            Stream rs = response.GetResponseStream();
            rs.CopyTo(fs);
            rs.Close();
            fs.Close();
            Title = vkAudio.title;
            Artist = vkAudio.artist;
        }
        internal mpLocalAudio(string filename, string artist, string title)
        {
            Link = mpDataBaseController.Instance().generateFileName();
            File.Copy(filename, Link);
            Artist = artist;
            Title = title;
        }
        internal mpLocalAudio() { }
        internal override XElement toXElement()
        {
            XElement result = base.toXElement();
            result.Add(
                new XAttribute("type", "local"),
                new XAttribute("filename", Link)
                );
            return result;
        }
        new internal static mpLocalAudio fromXElement(XElement element)
        {
            mpLocalAudio result = mpAudio.fromXElement(element) as mpLocalAudio;
            result.Link = element.Attribute("filename").Value;
            return result;
        }
    }
    public class mpAudioLink : mpAudio
    {
        public string VKID { get; internal set; }
        internal mpAudioLink(PashaAudio vkAudio)
        {
            VKID = vkAudio.aid;
            Link = vkAudio.url;
            Artist = vkAudio.artist;
            Title = vkAudio.title;
        }
        private mpAudioLink() { }
        internal override XElement toXElement()
        {
            XElement result = base.toXElement();
            result.Add(
                new XAttribute("type", "link"),
                new XAttribute("url", Link),
                new XAttribute("vkId", VKID)
                );
            return result;
        }
        new internal static mpAudioLink fromXElement(XElement element)
        {
            mpAudioLink result = mpAudio.fromXElement(element) as mpAudioLink;
            result.Link = element.Attribute("url").Value;
            result.VKID = element.Attribute("vkId").Value;
            return result;
        }
    }
    public class mpLocalAudioLink : mpAudio
    {
        internal mpLocalAudioLink(string filename, string artist, string title)
        {
            Link = filename;
            Artist = artist;
            Title = title;
        }
        private mpLocalAudioLink() { }
        internal override XElement toXElement()
        {
            XElement result = base.toXElement();
            result.Add(
                new XAttribute("type", "link"),
                new XAttribute("filename", Link)
                );
            return result;
        }
        new internal static mpLocalAudioLink fromXElement(XElement element)
        {
            mpLocalAudioLink result = mpAudio.fromXElement(element) as mpLocalAudioLink;
            result.Link = element.Attribute("filename").Value;
            return result;
        }
    }
    public class mpAudio
    {
        /// <summary>
        /// Internal for collection identifier.
        /// </summary>
        public int Id { get; internal set; }
        public string Link { get; internal set; }
        public string Artist { get; internal set; }
        public string Title { get; internal set; }
        internal virtual XElement toXElement()
        {
            XElement result = new XElement("track");
            result.Add(
                new XAttribute("id", Id),
                new XAttribute("author", Artist),
                new XAttribute("name", Title)
                );
            return result;
        }
        internal static mpAudio fromXElement(XElement element)
        {
            mpAudio audio;
            if (element.Attribute("type").Value == "link")
                audio = mpAudioLink.fromXElement(element);
            else

                audio = mpLocalAudio.fromXElement(element);
            audio.Artist = element.Attribute("author").Value;
            audio.Title = element.Attribute("name").Value;
            audio.Id = int.Parse(element.Attribute("id").Value);
            return audio;
        }
    }
}

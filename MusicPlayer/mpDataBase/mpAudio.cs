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
        string Url { get; }
        string Author { get; }
        string Name { get; }
        string VKID { get; }
    }
    public class mpLocalAudio : mpAudio
    {
        public string FileName { get; internal set; }
        internal mpLocalAudio(PashaAudio vkAudio)
        {
            string targetFile = mpDataBaseController.generateFileName();
            HttpWebResponse response =
              (HttpWebResponse)((HttpWebRequest)WebRequest.Create(vkAudio.Url)).GetResponse();
            FileStream fs = new FileStream(targetFile, FileMode.Create);
            Stream rs = response.GetResponseStream();
            rs.CopyTo(fs);
            FileName = targetFile;
            rs.Close();
            fs.Close();
            Name = vkAudio.Name;
            Author = vkAudio.Author;
        }
        internal mpLocalAudio(string filename)
        {
            //Todo creating audio from local
        }
        internal mpLocalAudio() { }
        internal override XElement toXElement()
        {
            XElement result = base.toXElement();
            result.Add(
                new XAttribute("type", "local"),
                new XAttribute("filename", FileName)
                );
            return result;
        }
        new internal static mpLocalAudio fromXElement(XElement element)
        {
            mpLocalAudio result = new mpLocalAudio();
            result.FileName = element.Attribute("filename").Value;
            return result;
        }
    }
    public class mpAudioLink : mpAudio
    {
        public string Url { get; internal set; }
        public string VKID { get; internal set; }
        internal mpAudioLink(PashaAudio vkAudio)
        {
            VKID = vkAudio.VKID;
            Url = vkAudio.Url;
            Author = vkAudio.Author;
            Name = vkAudio.Name;
        }
        private mpAudioLink() { }
        internal override XElement toXElement()
        {
            XElement result = base.toXElement();
            result.Add(
                new XAttribute("type", "link"),
                new XAttribute("url", Url),
                new XAttribute("vkId", VKID)
                );
            return result;
        }
        new internal static mpAudioLink fromXElement(XElement element)
        {
            mpAudioLink result = new mpAudioLink();
            result.Url = element.Attribute("url").Value;
            result.VKID = element.Attribute("vkId").Value;
            return result;
        }
    }
    public class mpLocalAudioLink : mpAudio
    {
        public string Filename { get; internal set; }
        internal mpLocalAudioLink(PashaAudio vkAudio)
        {
            Filename = vkAudio.Url;
            Author = vkAudio.Author;
            Name = vkAudio.Name;
        }
        private mpLocalAudioLink() { }
        internal override XElement toXElement()
        {
            XElement result = base.toXElement();
            result.Add(
                new XAttribute("type", "link"),
                new XAttribute("filename", Filename)
                );
            return result;
        }
        new internal static mpLocalAudioLink fromXElement(XElement element)
        {
            mpLocalAudioLink result = new mpLocalAudioLink();
            result.Filename = element.Attribute("filename").Value;
            return result;
        }
    }
    public abstract class mpAudio
    {
        /// <summary>
        /// Internal for collection identifier.
        /// </summary>
        public int Id { get; internal set; }
        public string Author { get; internal set; }
        public string Name { get; internal set; }
        internal virtual XElement toXElement()
        {
            XElement result = new XElement("track");
            result.Add(
                new XAttribute("id", Id),
                new XAttribute("author", Author),
                new XAttribute("name", Name)
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
            audio.Author = element.Attribute("author").Value;
            audio.Name = element.Attribute("name").Value;
            audio.Id = int.Parse(element.Attribute("id").Value);
            return audio;
        }
    }
}

﻿using System.IO;
using System.Net;
using System.Xml.Linq;

namespace mpDataBase
{
    /// <summary>
    /// Created for attaching another part of project
    /// </summary>
    public interface PashaAudio
    {
        string url { get; }
        string artist { get; }
        string title { get; }
        string aid { get; }
    }
    /// <summary>
    /// Describes audio, located in workpath.
    /// </summary>
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
    /// <summary>
    /// Describes audio, located on external web-server. <c>Link</c> method gives url instead of filename.
    /// </summary>
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
    /// <summary>
    /// Describes audio, located somewhere on local machine, excluding workpath.
    /// </summary>
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
    /// <summary>
    /// Base class for all audio classes.
    /// </summary>
    public abstract class mpAudio
    {
        /// <summary>
        /// Internal for collection identifier.
        /// </summary>
        public int Id { get; internal set; }
        /// <summary>
        /// Refers to audiofile location.
        /// </summary>
        public string Link { get; internal set; }
        public string Artist { get; internal set; }
        public string Title { get; internal set; }
        //internal mpAudio() { }
        /// <summary>
        /// Serialization method.
        /// </summary>
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
        /// <summary>
        /// Deserialization method.
        /// </summary>
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

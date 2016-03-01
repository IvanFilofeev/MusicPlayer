using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using System.Xml;

namespace mpDataBase
{
    public partial class mpDataBaseController
    {
        private void Deserialize()
        {
            XDocument doc = XDocument.Load(xmlDataFile);
            XElement common = doc.Root.Element("commonData");
            maxId = int.Parse(common.Element("maxId").Value);
            MusicFolder = common.Element("musicFolder").Value;
            XElement tracks = doc.Root.Element("tracks");
            foreach(XElement track in tracks.Elements())
                audioList.Add(mpAudio.fromXElement(track));
        }
        /// <summary>
        /// Saves state of singltone to .xml file.
        /// Does not offence data in object, so can be called periodically to save data in case of unexpected shutdown.
        /// </summary>
        public void Serialize()
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("MusicPlayerData");
            doc.Add(root);
            XElement common = new XElement("commonData");
            common.Add(new XElement("maxId", maxId));
            common.Add(new XElement("musicFolder", MusicFolder));
            root.Add(common);
            XElement tracks = new XElement("tracks");
            foreach (mpAudio audio in audioList)
                tracks.Add(audio.toXElement());
            root.Add(tracks);
            if (!Directory.Exists(xmlDataDirectory))
                Directory.CreateDirectory(xmlDataDirectory);
            doc.Save(xmlDataFile);
        }
        private void Init()
        {
            MusicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            maxId = -1;
            FileStream fs;
            mpLocalAudio audio;
            foreach(string file in MP3List(MusicFolder))
            {
                audio = new mpLocalAudio();
                TagLib.File audioFile = TagLib.File.Create(file);
                audio.Author = audioFile.Tag.Performers[0];
                audio.Name = audioFile.Tag.Title;
                audio.Id = ++maxId;
                audio.FileName = file;
                fs = new FileStream(file, FileMode.Open);
                fs.Close();
                audioList.Add(audio);
            }
        }
    }
    //just comment with no meaning
}

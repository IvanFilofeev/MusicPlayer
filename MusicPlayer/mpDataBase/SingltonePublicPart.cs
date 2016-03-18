using System.Collections.Generic;
using System.IO;

namespace mpDataBase
{
    public partial class mpDataBaseController
    {
        public List<mpAudio> AudioList()
        {
            return new List<mpAudio>(audioList);
        }
        /// <summary>
        /// Directory to save music, default is user's MyMusic folder.
        /// The set method also creates MusicFolder\cache folder,
        /// where all added through <c>addAudio</c> files will be saved.
        /// Doesn't move files.
        /// </summary>
        public string MusicFolder
        {
            get { return musicFolder; }
            set
            {
                Directory.CreateDirectory(value + "\\cache");
                musicFolder = value;
            }
        }
        public enum AudioType { atFile, atLink };
        /// <summary>
        /// Adds audio from vk.com to collection.
        /// </summary>
        /// <param name="vkAudio">Audio to add.</param>
        /// <param name="audioType">Set AudioType.atFile to save locally not only link, but also .mp3 itself.</param>
        /// <returns>Returns <c>mpAudio</c> instance that refers to the new element of collection.</returns>
        public mpAudio addAudio(PashaAudio vkAudio, AudioType audioType = AudioType.atLink)
        {
            mpAudio audio;
            if (audioType == AudioType.atFile)
                audio = new mpAudioLink(vkAudio);
            else
                audio = new mpLocalAudio(vkAudio);
            audio.Id = ++maxId;
            if (audio.Id == int.MaxValue)
                OptimizeIndex();
            return audio;
        }
        /// <summary>
        /// Adds audio from local computer to collection.
        /// </summary>
        /// <param name="filename">File to add</param>
        /// <param name="artist, title">Tags for file</param>
        /// <param name="audioType">If atFile, copies audio to working directory, it's also default behavior.
        /// If atLink, saves filename and uses it to access file later.</param>
        /// <returns>Returns <c>mpAudio</c> instance that refers to the new element of collection.</returns>
        public mpAudio addAudio(string filename, string artist, string title, AudioType audioType = AudioType.atFile)
        {
            mpAudio audio;
            if (audioType == AudioType.atLink)
                audio = new mpLocalAudioLink(filename, artist, title);
            else
                audio = new mpLocalAudio(filename, artist, title);
            audio.Id = ++maxId;
            if (audio.Id == int.MaxValue)
                OptimizeIndex();
            return audio;
        }
    }
}

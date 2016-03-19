using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Environment;

namespace mpDataBase
{
    public partial class mpDataBaseController
    {
        string xmlDataFile = GetFolderPath(SpecialFolder.ApplicationData) + "\\MusicPlayer\\data.xml";
        string xmlDataDirectory = GetFolderPath(SpecialFolder.ApplicationData) + "\\MusicPlayer";
        List<mpAudio> audioList = new List<mpAudio>();
        int maxId;
        private string musicFolder;
        private void OptimizeIndex()
        {
            int i = 0;
            foreach (mpAudio audio in audioList)
                audio.Id = i++;
        }
        internal string generateFileName()
        {
            long x;
            Random rand = new Random();
            do
                x = rand.Next() * int.MaxValue + rand.Next();
            while (File.Exists(musicFolder + "\\cache\\track" + x + ".mp3"));
            return musicFolder + "\\cache\\track" + x + ".mp3";
        }
        internal static string MD5Hash(Stream s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(s);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));
            return sb.ToString();
        }
        private static List<string> MP3List(string dir)
        {
            List<string> result = new List<string>();
            string[] files = Directory.GetFiles(dir);
            string[] subdirs = Directory.GetDirectories(dir);
            foreach (string subdir in subdirs)
                result.AddRange(MP3List(subdir));
            foreach (string file in files)
                if(file.Substring(file.LastIndexOf("."))==".mp3")
                    result.Add(file);
            return result;
        }
    }
}

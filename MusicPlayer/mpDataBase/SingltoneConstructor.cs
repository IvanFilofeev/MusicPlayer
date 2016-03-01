﻿using System;
using System.IO;

namespace mpDataBase
{
    public partial class mpDataBaseController
    {
        static mpDataBaseController instance;
        private mpDataBaseController()
        {
            Console.WriteLine(xmlDataFile);
            if (File.Exists(xmlDataFile))
                Deserialize();
            else
                Init();
            Console.WriteLine();
        }
        public static mpDataBaseController Instance()
        {
            if (instance == null)
                instance = new mpDataBaseController();
            return instance;
        }
    }
}
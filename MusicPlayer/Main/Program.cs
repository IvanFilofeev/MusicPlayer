using mpDataBase;
using System;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(mpDataBaseController.Instance().MusicFolder);
            Console.ReadLine();
            mpDataBaseController.Instance().Serialize();
        }
    }
}

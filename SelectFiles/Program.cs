using System;
using System.IO;

namespace SelectFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists(@"filelist.txt"))
            {
                System.Console.WriteLine("Filelist does not exist");
            }

            System.IO.StreamReader file = new System.IO.StreamReader(@"filelist.txt");
            string strExeFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string DestPath = strExeFilePath + "\\Selected\\";
            string line;

            if (!Directory.Exists(DestPath))
            {
                Directory.CreateDirectory(DestPath);
            }

            while ((line = file.ReadLine()) != null)
            {
                string[] filelist = Directory.GetFiles(strExeFilePath, line);

                if (File.Exists(filelist[0]))
                {
                    File.Move(filelist[0], Path.Combine(DestPath, Path.GetFileName(filelist[0])));
                }
            }
        }
    }
}

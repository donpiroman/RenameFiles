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
            System.Console.WriteLine("Exe path {0}.", strExeFilePath);
            string DestPath = strExeFilePath + "\\Selected\\";
            System.Console.WriteLine("Destination path {0}", DestPath);

            string line;

            if (!Directory.Exists(DestPath))
            {
                Directory.CreateDirectory(DestPath);
            }

            while ((line = file.ReadLine()) != null)
            {
                line = Path.GetFileNameWithoutExtension(line);
                System.Console.WriteLine("File:" + line);
                string[] filelist = Directory.GetFiles(strExeFilePath, line+"*");
                if (filelist.Length > 0)
                {
                    if (File.Exists(filelist[0]))
                    {
                        File.Copy(filelist[0], Path.Combine(DestPath, Path.GetFileName(filelist[0])));
                        System.Console.WriteLine("File moved:" + filelist[0]);

                    }
                }
            }
            Console.ReadKey();
        }
    }
}

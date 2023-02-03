using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LookForFiles
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string rootPath = @"\\ciqsandystor2\CifsData3\Ebilling\ESIS\ACK\History\2023\01\20\";
            string destPath = @"C:\Users\BSantizo\Desktop\Files\";
            //C:\Users\BSantizo\Desktop\Files
            var filesFound = new List<string>(); 
            var filesNotFound = new List<string>();
            var fileAlreadyCopy = new List<string>();
            
            string line;
            try
            {
                if (!File.Exists(@"filelist.txt"))
                {
                    System.Console.WriteLine("Filelist does not exist");
                }
                System.IO.StreamReader file = new System.IO.StreamReader(@"filelist.txt");

                while ((line = file.ReadLine()) != null)
                {
                    string[] filelist = Directory.GetFiles(rootPath, line + "*", SearchOption.AllDirectories);
                    if (filelist.Length > 0)
                    {
                        if (File.Exists(filelist[0]))
                        {
                            if (!File.Exists(Path.Combine(destPath, Path.GetFileName(filelist[0]))))
                            {
                                File.Copy(filelist[0], Path.Combine(destPath, Path.GetFileName(filelist[0])));
                                filesFound.Add(Path.GetFileName(filelist[0]));
                                System.Console.WriteLine("File found: " + Path.GetFileName(filelist[0]));
                            }
                            else
                            {
                                fileAlreadyCopy.Add(line);
                            }
                        }
                    }
                    else
                    {
                        filesNotFound.Add(line);
                        System.Console.WriteLine("File not found: " + line);
                    }
                }
                await writeFileAsync(filesFound, "FilesFound.txt");
                await writeFileAsync(filesNotFound, "FilesNotFound.txt");
                await writeFileAsync(fileAlreadyCopy, "fileAlreadyCopy.txt");
            }
            catch(Exception ex )
            {
                System.Console.WriteLine("ERROR: " + ex.Message);

            }

        }

        public static async Task writeFileAsync(List<string> lines, string filename)
        {
            using StreamWriter file = new(filename);

            foreach (string line in lines)
            {
                await file.WriteLineAsync(line);
            }
            await file.FlushAsync();
            file.Close();
        }
    }
}

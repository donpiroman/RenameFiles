using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LookForFiles
{
    class Program
    {
        static async Task Main(string[] args)
        {

            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();

            //Console.WriteLine($"Hello, { root["weather"] } world!");

            string rootPath = root["sourcepath"];
            string destPath = root["destinationpath"];
            string fileList = root["filelist"];
            //C:\Users\BSantizo\Desktop\Files
            var filesFound = new List<string>(); 
            var filesNotFound = new List<string>();
            var fileAlreadyCopy = new List<string>();
            
            string line;
            try
            {
                if (!File.Exists(fileList))
                {
                    System.Console.WriteLine("Filelist does not exist");
                }
                System.IO.StreamReader file = new System.IO.StreamReader(fileList);

                while ((line = file.ReadLine()) != null)
                {
                    string[] filelist = Directory.GetFiles(rootPath, "*"+line+"*", SearchOption.AllDirectories);
                    if (filelist.Length > 0)
                    {
                        if (File.Exists(filelist[0]))
                        {
                            if (!File.Exists(Path.Combine(destPath, Path.GetFileName(filelist[0]))))
                            {
                                File.Copy(filelist[0], Path.Combine(destPath, Path.GetFileName(filelist[0])),true);
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
                await writeFileAsync(filesFound, Path.Combine(Path.GetDirectoryName(fileList),  "FilesFound.txt"));
                await writeFileAsync(filesNotFound, Path.Combine(Path.GetDirectoryName(fileList), "FilesNotFound.txt"));
                await writeFileAsync(fileAlreadyCopy, Path.Combine(Path.GetDirectoryName(fileList), "fileAlreadyCopy.txt"));
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

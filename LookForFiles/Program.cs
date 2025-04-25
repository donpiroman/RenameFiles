using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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

                DirectoryInfo directory = new DirectoryInfo(rootPath);
                //var fileZipList = directory.GetFiles("*.zip",SearchOption.AllDirectories).Select(c => new { c.FullName, c.Name }).ToList(); //Directory.GetFiles(rootPath, line, SearchOption.TopDirectoryOnly);
                //var fileZipList = directory.GetFiles("*.zip", SearchOption.AllDirectories).ToList(); //Directory.GetFiles(rootPath, line, SearchOption.TopDirectoryOnly);

                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim();
                    
                    var zipFiles = directory.GetFiles($"{line}*.pdf", SearchOption.AllDirectories).Select(c => new { c.FullName, c.Name }).ToList(); //fileZipList.Where(c => c.Name.Contains(line)).ToList();
                    if (zipFiles.Count <= 0 )
                    {
                        filesNotFound.Add(line);
                        System.Console.WriteLine("File not found: " + line);
                        continue;
                    }
                    foreach (var zipFile in zipFiles)
                    {
                        var fullZipFile = zipFile.FullName;  //Path.Combine(rootPath, zipFile);
                        if (File.Exists(fullZipFile))
                        {
                            if (!File.Exists(Path.Combine(destPath, Path.GetFileName(fullZipFile))))
                            {
                                File.Copy(fullZipFile, Path.Combine(destPath, Path.GetFileName(fullZipFile)), true);
                                filesFound.Add(Path.GetFileName(fullZipFile));
                                System.Console.WriteLine("File found: " + Path.GetFileName(fullZipFile));
                            }
                            else
                            {
                                fileAlreadyCopy.Add(line);
                            }
                        }
                        else
                        {
                            filesNotFound.Add(line);
                            System.Console.WriteLine("File not found: " + line);
                        }
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

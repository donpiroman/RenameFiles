using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AMC_RenameFiles
{
    class Program
    {
        public static string getBetween(string strSource, string strStart,string addEndText)
        {
           
            int InitPosFound = strSource.IndexOf(strStart);
            int lenghtSource = 0;

            if (InitPosFound > 0)
            {
                lenghtSource = strSource.Length - strStart.Length;


                return strSource.Substring(0,InitPosFound) + addEndText;
            }
            else
            {
                return String.Empty;
            }
        }
        static void Main(string[] args)
        {
            int counter = 0;
            string line;
            string newline;

            string startSplit = "ORIG";
            //string startSplit = "";
            //string Replacewith = "_ReScan";
            string Replacewith = "ReScan";



            if (!File.Exists(@"filelist.txt"))
            {
                System.Console.WriteLine("Filelist does not exist");
            }
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(@"filelist.txt");
            string strExeFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string DestPath = strExeFilePath+ "\\Procesados\\";

            while ((line = file.ReadLine()) != null)
            {
                string[] filelist = Directory.GetFiles(strExeFilePath, line + "*");


                if (File.Exists(@"" + filelist[0]))
                {
                    newline = getBetween(filelist[0], startSplit, Replacewith  + Path.GetExtension(filelist[0]));

                    if ( !Directory.Exists(DestPath))
                    {
                        Directory.CreateDirectory(DestPath);
                    }

                    File.Move(filelist[0], Path.Combine(DestPath, Path.GetFileName(newline)));
                        
                    System.Console.WriteLine(filelist[0] + " => "+ Path.Combine(DestPath, Path.GetFileName(newline)));
                }
                counter++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();
            
            
        }
    }
}

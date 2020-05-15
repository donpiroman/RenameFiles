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

            string startSplit = "_ORIG";



            if (File.Exists(@"filelist.txt"))
            {
                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader(@"filelist.txt");
                while ((line = file.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);
                    if (File.Exists(@""+line))
                    {
                        newline = getBetween(line, startSplit, "_ReScan" + Path.GetExtension(line));

                       if ( !Directory.Exists(System.IO.Directory.GetCurrentDirectory()+@"\Procesados"))
                        {
                            Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Procesados");
                        }

                        File.Move(line, System.IO.Directory.GetCurrentDirectory() + @"\Procesados\" + newline);
                        
                        System.Console.WriteLine(line);
                    }
                    counter++;
                }

                file.Close();
                System.Console.WriteLine("There were {0} lines.", counter);
                // Suspend the screen.  
                System.Console.ReadLine();
            }
            else
            {
                System.Console.WriteLine("Filelist does not exist");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BatchFileRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            ///Assembly references the current file where the actual code locates
            String path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            String from;
            String to;
            bool doRecursive=false;
            if (args.Length == 0)
            {
                showUsage();
                Console.WriteLine("stringToMatch: ");
                from = Console.ReadLine();
                Console.WriteLine("stringToSet: ");
                to = Console.ReadLine();
                Console.WriteLine("recursive?: ");
                doRecursive = Console.ReadLine().Equals("1");
            }
            else if(args.Length>=2)
            {
                from = args[0];
                to = args[1];
                try
                {
                    if (args[2] == "-r")
                    {
                        doRecursive = (args[3].Equals("1") ? true : false);
                    }
                }
                catch(IndexOutOfRangeException e)
                {
                    doRecursive = false;
                }
            }
            else
            {
                showUsage();
                return;
            }
            if (path[path.Length - 1] != '\\')
            {
                path += '\\';
            }
            Program main = new Program();
            main.getFilesInPath(path, from, to, doRecursive);
            Console.WriteLine("Press any key to exit");
            Console.Read();

        }
        void getFilesInPath(String path, String from, String to, bool doRecursive)
        {
            string match = "*.*";
            string[] files = Directory.GetFiles(path, match, (doRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            List<FileInfo> toReturn = new List<FileInfo>();
            Console.WriteLine("Files matched:");
            foreach (String file in files)
            {
                FileInfo actualFile = new FileInfo(file);
                if (!actualFile.FullName.Equals(actualFile.FullName.Replace(from, to))){
                    Console.WriteLine(actualFile.FullName.Replace(from, to));
                    try
                    {
                        actualFile.MoveTo(actualFile.FullName.Replace(from, to));
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("Cannot move " + actualFile.Name);
                    }
                }
            }
        }
        static void showUsage()
        {
            Console.WriteLine("Usage(only to the files that matches):");
            Console.WriteLine("BFR stringToMatch stringToSet [-r (0|1)]");
            Console.WriteLine("-r: search in subfolders too, 0: false(default) / 1: true");
        }
    }
}

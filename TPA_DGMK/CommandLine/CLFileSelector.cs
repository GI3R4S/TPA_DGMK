using System;
using System.IO;
using ViewModel;

namespace CommandLine
{
    internal class CLFileSelector : IFileSelector
    {
        public string SelectSource()
        {
            string path = "";
            do
            {
                Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
                Console.WriteLine("Insert path of .dll file.");
                string loadedPath = Console.ReadLine();


                if (!File.Exists(loadedPath))
                {
                    Console.WriteLine("There's no file at given path.\n Press any key to retry, ESC to end program");
                    if (Console.ReadKey().Key != ConsoleKey.Escape)
                    {
                        Console.Clear();
                        continue;
                    }

                    Environment.Exit(-1);
                }
                else if (!loadedPath.EndsWith(".dll"))
                {
                    Console.WriteLine("Selected file doesn't have correct extension\n Press any key to retry, ESC to end program");
                    if (Console.ReadKey().Key != ConsoleKey.Escape)
                    {
                        Console.Clear();
                        continue;
                    }

                    Environment.Exit(-1);
                }

                path = loadedPath;
                break;
            } while (true);

            return path;
        }
    }
}

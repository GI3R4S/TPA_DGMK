using System;
using System.IO;
using ViewModel;

namespace CommandLine
{
    internal class CLFileSelector : IFileSelector
    {
        public string SelectSource()
        {
            Console.Clear();
            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
            Console.WriteLine("Insert path of file.");
            return Console.ReadLine();       
        }

        public void FailureAlert()
        {
            Console.WriteLine("File at chosen path doesn't exist or has incorrect extension");
            Console.WriteLine("Press any key to retry...");
            if (Console.ReadKey() != null)
                return;
        }
    }
}

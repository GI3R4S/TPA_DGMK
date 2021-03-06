﻿using System;
using System.ComponentModel.Composition;
using System.IO;
using ViewModel;

namespace ClFileSelector
{
    [Export(typeof(IFileSelector))]
    internal class CLFileSelector : IFileSelector
    {
        public string SelectSource()
        {
            Console.Clear();
            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
            Console.WriteLine("Insert path of file.");
            return Console.ReadLine();
        }

        public string SelectTarget()
        {
            Console.Clear();
            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
            Console.WriteLine("Insert path of file, if file does not exist it will be created.");
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
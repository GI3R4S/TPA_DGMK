using System;
using ViewModel;

namespace CommandLine
{
    internal class CLFileSelector : IFileSelector
    {
        public string SelectSource()
        {
            return Console.ReadLine();
        }
    }
}

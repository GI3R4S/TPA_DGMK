using System;
using System.IO;
using ViewModel;

namespace CommandLine
{
    class CLDatabaseSelector : IFileSelector
    {
        public string SelectSource()
        {
            Console.Clear();
            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
            Console.WriteLine("Insert name of database.");
            return "Data source=.;Initial catalog=" + Console.ReadLine()
                + ";integrated security=true;persist security info=True;";
        }

        public string SelectTarget()
        {
            Console.Clear();
            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
            Console.WriteLine("Insert name of database, if database does not exist it will be created.");
            return "Data source=.;Initial catalog=" + Console.ReadLine()
                + ";integrated security=true;persist security info=True;";
        }

        public void FailureAlert()
        {

        }
    }
}

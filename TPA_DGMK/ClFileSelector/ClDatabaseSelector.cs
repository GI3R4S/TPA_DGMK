using System;
using System.ComponentModel.Composition;
using System.IO;
using ViewModel;

namespace ClFileSelector
{
    [Export(typeof(IDatabaseSelector))]
    class ClDatabaseSelector : IDatabaseSelector
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
    }
}

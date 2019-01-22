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
            return "Data source=(LocalDb)\\MSSQLLocalDB;" + GetPath() + ";integrated security=true;persist security info=True;";
        }

        public string SelectTarget()
        {
            Console.Clear();
            return "Data source=(LocalDb)\\MSSQLLocalDB;" + GetPath() + ";integrated security=true;persist security info=True;";
        }

        public string GetPath()
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            path = path.Remove(path.Length - 24);
            return "AttachDbFilename=" + path + "ModelDB\\DatabaseForSerialization.mdf";
        }
    }
}

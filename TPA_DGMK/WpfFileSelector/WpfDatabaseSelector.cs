﻿using System.ComponentModel.Composition;
using ViewModel;

namespace WpfFileSelector
{
    [Export(typeof(IDatabaseSelector))]
    public class WpfDatabaseSelector : IDatabaseSelector
    {
        public string SelectTarget()
        {
            return "Data source=(LocalDb)\\MSSQLLocalDB;" + GetPath() + ";integrated security=true;persist security info=True;";
        }

        public string SelectSource()
        {
            return "Data source=(LocalDb)\\MSSQLLocalDB;" + GetPath() + ";integrated security=true;persist security info=True;";
        }

        public string GetPath()
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            path = path.Remove(path.Length - 25);
            return "AttachDbFilename=" + path + "ModelDB\\DatabaseForSerialization.mdf";
        }
    }
}

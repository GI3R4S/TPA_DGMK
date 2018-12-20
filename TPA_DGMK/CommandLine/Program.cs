using CommandLine;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;

namespace CommandLineInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            NameValueCollection paths = (NameValueCollection)ConfigurationManager.GetSection("paths");
            string[] pathsCatalogs = paths.AllKeys;
            List<DirectoryCatalog> directoryCatalogs = new List<DirectoryCatalog>();
            foreach (string pathsCatalog in pathsCatalogs)
            {
                if (Directory.Exists(pathsCatalog))
                    directoryCatalogs.Add(new DirectoryCatalog(pathsCatalog));
            }

            AggregateCatalog catalog = new AggregateCatalog(directoryCatalogs);
            CompositionContainer container = new CompositionContainer(catalog);
            CLView view = new CLView();
            container.ComposeParts(view.ViewModel);
            view.Run();
        }
    }
}

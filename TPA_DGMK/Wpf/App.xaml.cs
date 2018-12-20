using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Windows;
using ViewModel;

namespace Wpf
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
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
            ViewModelBase vm = new ViewModelBase();
            MainWindow window = new MainWindow
            {
                DataContext = vm
            };
            container.ComposeParts(vm);
            window.Show();
            Application.Current.MainWindow = window;
        }
    }
}

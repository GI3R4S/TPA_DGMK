using Data_De_Serialization;
using Logging;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows;
using ViewModel;

namespace Wpf
{
    public partial class App : Application
    {
        private static Logger logger;
        private static SerializerTemplate serializer;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CompositionContainer container;
            AssemblyCatalog assemblyCatalog = new AssemblyCatalog(typeof(App).Assembly);
            AggregateCatalog catalog = new AggregateCatalog(assemblyCatalog,
                new DirectoryCatalog(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            container = new CompositionContainer(catalog);
            List<Logger> loggers = container.GetExportedValues<Logger>().ToList();
            logger = loggers.FirstOrDefault(component => component.ToString().Contains(ConfigurationManager.AppSettings["loggingComponent"]));
            List<SerializerTemplate> serializers = container.GetExportedValues<SerializerTemplate>().ToList();
            serializer = serializers.FirstOrDefault(component => component.ToString().Contains(ConfigurationManager.AppSettings["serializingComponent"]));

            container.ComposeParts(this);
            ViewModelBase vm = new ViewModelBase(new WpfFileSelector(), new WpfDatabaseSelector(), logger, serializer);
            MainWindow window = new MainWindow
            {
                DataContext = vm
            };
            window.Show();
            Application.Current.MainWindow = window;
        }
    }
}

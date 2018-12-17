using CommandLine;
using Data_De_Serialization;
using Logging;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CommandLineInterface
{
    class Program
    {
        private static Logger logger;
        private static SerializerTemplate serializer;

        static void Main(string[] args)
        {
            CompositionContainer container;
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

            container = new CompositionContainer(catalog);
            List<Logger> loggers = container.GetExportedValues<Logger>().ToList();
            logger = loggers.FirstOrDefault(component => component.ToString().Contains(ConfigurationManager.AppSettings["loggingComponent"]));
            List<SerializerTemplate> serializers = container.GetExportedValues<SerializerTemplate>().ToList();
            serializer = serializers.FirstOrDefault(component => component.ToString().Contains(ConfigurationManager.AppSettings["serializingComponent"]));
            CLView view = new CLView(logger, serializer);
            container.ComposeParts(view);
            view.Run();
        }
    }
}

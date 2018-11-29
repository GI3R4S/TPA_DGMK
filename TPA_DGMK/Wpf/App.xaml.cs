using Data_De_Serialization;
using Logging;
using System.Windows;
using ViewModel;

namespace Wpf
{
    public partial class App : Application
    {
        public Logger logger { get; set; }
        public SerializerTemplate serializer { get; set; }
        public IFileSelector fileSelector{ get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            fileSelector = new WpfFileSelector();
            logger = new FileLogger();
            serializer = new XMLSerializer();
            ViewModelBase vm = new ViewModelBase(fileSelector, logger, serializer);
            MainWindow window = new MainWindow();
            window.DataContext = vm;
            window.Show();
        }
    }
}

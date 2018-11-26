using System;
using System.Windows;
using Data_De_Serialization;
using Logging;
using ViewModel;

namespace Wpf
{
    public partial class App : Application
    {
        public Logger logger { get; set; }
        public SerializerTemplate<Object> serializer { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            logger = new FileLogger();
            IFileSelector fileSelector = new WpfFileSelector();
            serializer = new XMLSerializer<Object>();
            ViewModelBase vm = new ViewModelBase(fileSelector, logger, serializer);
            MainWindow window = new MainWindow();
            window.DataContext = vm;
            window.Show();
        }
    }
}

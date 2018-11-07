using System.Windows;
using Logging;
using ViewModel;

namespace Wpf
{
    public partial class App : Application
    {
        public Logger Logger { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Logger = new FileLogger();
            IFileSelector fileSelector = new WpfFileSelector();
            ViewModelBase vm = new ViewModelBase(fileSelector, Logger);
            MainWindow window = new MainWindow();
            window.DataContext = vm;
            window.Show();
        }
    }
}

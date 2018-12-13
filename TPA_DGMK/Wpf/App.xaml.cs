using System.Windows;
using ViewModel;

namespace Wpf
{
    public partial class App : Application
    {
        public IFileSelector fileSelector{ get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            fileSelector = new WpfFileSelector();
            ViewModelBase vm = new ViewModelBase(fileSelector);
            vm.DatabaseSelector = new WPFDatabaseSelector();
            MainWindow window = new MainWindow();
            window.DataContext = vm;
            window.Show();
            Application.Current.MainWindow = window;
        }
    }
}

using System.Windows;
using System.Windows.Media;

namespace Wpf
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            BrushConverter brushConverter = new BrushConverter();
            grid.Background = (Brush)brushConverter.ConvertFrom("#272727");
            TreeView.Background = (Brush)brushConverter.ConvertFrom("#272727");
        }
    }
}

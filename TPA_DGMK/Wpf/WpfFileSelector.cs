using ViewModel;
using System.Windows.Forms;

namespace Wpf
{
    internal class WpfFileSelector : IFileSelector
    {
        public string SelectSource()
        {
            string path = null;
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = dialog.FileName;
            }
            return path;
        }
    }
}

using ViewModel;
using System.Windows.Forms;

namespace Wpf
{
    internal class WpfFileSelector : IFileSelector
    {
        public string SelectSource()
        {
            string path = null;
            do
            {
                OpenFileDialog dialog = new OpenFileDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    path = dialog.FileName;
                }

                if (!path.EndsWith(".dll"))
                {
                    MessageBox.Show("Selected file doesn't have .dll extension. Please retry selection.");
                }
                else
                {
                    break;
                }
            } while (true);

            return path;
        }
    }
}

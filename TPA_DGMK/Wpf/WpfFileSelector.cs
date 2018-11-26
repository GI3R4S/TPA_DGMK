using System.Windows.Forms;
using ViewModel;

namespace Wpf
{
    internal class WpfFileSelector : IFileSelector
    {
        public string SelectSource()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            return dialog.FileName;
        }
        
        public string SelectTarget()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            return dialog.FileName;
        }

        public void FailureAlert()
        {
            DialogResult result = MessageBox.Show("File at chosen path doesn't exist or has incorrect extension", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel)
            {
                System.Environment.Exit(0);
            }
        }
    }
}

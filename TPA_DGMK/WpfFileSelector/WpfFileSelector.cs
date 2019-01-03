using System.ComponentModel.Composition;
using System.Windows.Forms;
using ViewModel;

namespace WpfFileSelector
{
    [Export(typeof(IFileSelector))]
    internal class WpfFileSelector : IFileSelector
    {
        public string SelectSource()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true
            };
            DialogResult result = dialog.ShowDialog();
            return dialog.FileName;
        }

        public string SelectTarget()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = false
            };
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

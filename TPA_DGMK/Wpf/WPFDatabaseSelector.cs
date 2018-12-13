using System.Windows.Forms;
using ViewModel;
using Microsoft.VisualBasic;

namespace Wpf
{
    public class WPFDatabaseSelector : IFileSelector
    {
        public string SelectTarget()
        {
            return "Data source=.;Initial catalog=" + Interaction.InputBox("Enter target database name:", "TPA - reflector", "", 0, 0)
                + ";integrated security=true;persist security info=True;";
        }

        public string SelectSource()
        {
            return "Data source=.;Initial catalog=" + Interaction.InputBox("Enter source database name:", "TPA - reflector", "", 0, 0)
                + ";integrated security=true;persist security info=True;";
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

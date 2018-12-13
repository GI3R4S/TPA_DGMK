using ViewModel;
using Microsoft.VisualBasic;

namespace Wpf
{
    public class WpfDatabaseSelector : IFileSelector
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
        }
    }
}

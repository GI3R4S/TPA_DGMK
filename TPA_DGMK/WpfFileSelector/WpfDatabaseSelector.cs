using Microsoft.VisualBasic;
using System.ComponentModel.Composition;
using ViewModel;

namespace WpfFileSelector
{
    [Export(typeof(IDatabaseSelector))]
    public class WpfDatabaseSelector : IDatabaseSelector
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
    }
}

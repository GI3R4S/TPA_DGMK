using System.ComponentModel.Composition;
using System.Windows.Forms;
using ViewModel;

namespace WpfFileSelector
{
    [Export(typeof(IDisplay))]
    public class WpfDisplayInformation : IDisplay
    {
        public void DisplayInformation(string information)
        {
            string caption = "Information";
            MessageBox.Show(information, caption);
        }
    }
}

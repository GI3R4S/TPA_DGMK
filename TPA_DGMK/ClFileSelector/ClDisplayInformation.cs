using System;
using System.ComponentModel.Composition;
using System.Threading;
using ViewModel;

namespace ClFileSelector
{
    [Export(typeof(IDisplay))]
    class ClDisplayInformation : IDisplay
    {
        public void DisplayInformation(string information)
        {
            Console.Write(information);
            Thread.Sleep(500);
            ClearLine();
        }

        private void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}

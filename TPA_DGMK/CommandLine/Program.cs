using CommandLine;

namespace CommandLineInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            CLView view = new CLView();
            view.Run();
        }
    }
}

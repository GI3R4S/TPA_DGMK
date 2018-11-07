using CommandLine;
using Logging;

namespace CommandLineInterface
{
    class Program
    {
        private static Logger logger;
        static void Main(string[] args)
        {
            logger = new FileLogger();
            CLView view = new CLView(logger);
            logger.Write(SeverityEnum.Information, "The program has been started");
            view.Run();
            logger.Write(SeverityEnum.Information, "The program has been ended");
        }
    }
}

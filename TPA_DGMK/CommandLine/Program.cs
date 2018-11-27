using CommandLine;
using Data_De_Serialization;
using Logging;
using System;

namespace CommandLineInterface
{
    class Program
    {
        private static Logger logger;
        private static SerializerTemplate serializer;

        static void Main(string[] args)
        {
            logger = new FileLogger();
            serializer = new XMLSerializer();
            CLView view = new CLView(logger, serializer);
            logger.Write(SeverityEnum.Information, "The program has been started");
            view.Run();
            logger.Write(SeverityEnum.Information, "The program has been ended");
        }
    }
}

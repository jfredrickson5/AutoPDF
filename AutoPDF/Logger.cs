using System;

namespace AutoPDF
{
    class Logger
    {

        private Logger() { }

        public static void Log(LogLevel level, string message)
        {
            if (level != LogLevel.Info || !Options.Instance.Verbose) return;
            Console.WriteLine("AutoPDF: " + message);
        }
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}

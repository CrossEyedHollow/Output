using System;

namespace ReportTools
{
    public static class Output
    {
        private readonly static char sep = System.IO.Path.DirectorySeparatorChar;

        /// <summary>
        /// If set to true adds a timestamp to the console messages
        /// </summary>
        public static bool ConsoleTimeStamp { get; set; } = true;
        /// <summary>
        /// Defines the Time Stamp format, default is "HH:mm:ss"
        /// </summary>
        public static string TimeStampFormat { get; set; } = "HH:mm:ss";
        /// <summary>
        /// Default FilePath is 'BaseDirectory/Logs'
        /// </summary>
        public static string FilePath { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}{sep}Logs";

        public static object Door { get; set; } = new object();

        /// <summary>
        /// Creates the directory/file if it doesn't exist, writes the message into the file and closes the file
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            lock (Door)
            {
                DateTime time = DateTime.Now;
                try
                {
                    System.IO.Directory.CreateDirectory(FilePath);
                    System.IO.File.AppendAllText($"{FilePath}{sep}{time.ToString("dd_MM_yyyy")}.log", $"[{time.ToString(TimeStampFormat)}]{message}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine((ConsoleTimeStamp ? $"[{time.ToString(TimeStampFormat)}]" : string.Empty) + "Failed to log event:  " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Calls the Log function and writes the message in the console
        /// </summary>
        /// <param name="msg"></param>
        public static void Report(string msg)
        {
            Log(msg);
            ToConsole(msg);
        }

        public static void ToConsole(string msg)
        {
            if (ConsoleTimeStamp) msg = $"[{DateTime.Now.ToString(TimeStampFormat)}]{msg}";
            Console.WriteLine(msg);
        }
    }
}

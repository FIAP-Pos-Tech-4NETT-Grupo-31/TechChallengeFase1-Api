
using Contatos.API.Interfaces;
using System.Reflection.PortableExecutable;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace Contatos.API.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly string FilePath;
        public LoggerService() 
        {
            var logFilename = $"log_{DateTime.Now.ToString("yyyyMMdd")}.txt";
            var filePath = Path.Combine(Environment.CurrentDirectory, "Logs", logFilename);

            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "Logs")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Logs"));
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }

            FilePath = filePath;
        }

        public void Error(string header, string message, Stopwatch? sw = null)
        {
            this.LogFileWrite("ERROR", header, message, sw);
        }

        public void Info(string header, string message, Stopwatch? sw = null)
        {
            this.LogFileWrite("INFO", header, message, sw);

        }

        public void Warn(string header, string message, Stopwatch? sw = null)
        {
            this.LogFileWrite("WARN", header, message, sw);
        }

        private void LogFileWrite(string logLevel, string header, string message, Stopwatch? sw = null)
        {
            if (sw != null)
                sw.Stop();

            var datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var stopWatchMessage = sw != null ? $"| {sw.ElapsedMilliseconds} milissegundos ".PadRight(25, ' ') : "";
            var text = $"{logLevel.PadRight(5, ' ')}| {header.PadRight(20, ' ')} {stopWatchMessage}| {datetime.PadRight(20, ' ')} | {message}";

            using (StreamWriter w = File.AppendText(this.FilePath))
                w.WriteLine(text);
        }
    }
}

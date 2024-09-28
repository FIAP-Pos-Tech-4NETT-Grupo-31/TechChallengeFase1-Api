
using Contatos.API.Interfaces;
using System.Reflection.PortableExecutable;
using Serilog;

namespace Contatos.API.Services
{
    public class LoggerService : ILoggerService
    {
          public void Error(string header, string message)
        {
            Log.Error($"{header} - {message}");
        }

        public void Info(string header, string message)
        {
            Log.Information($"{header} - {message}");
        }

        public void Warn(string header, string message)
        {
            Log.Information($"{header} - {message}");
        }
    }
}

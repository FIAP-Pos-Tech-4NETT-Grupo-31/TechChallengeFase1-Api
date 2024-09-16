using System.Diagnostics;

namespace Contatos.API.Interfaces
{
    public interface ILoggerService
    {        
        public void Info(string header, string message);
        public void Error(string header, string message);
        public void Warn(string header, string message);
    }
}

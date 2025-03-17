using HappyDesk.Domain.Constants;
using HappyDesk.Domain.Interfaces;

namespace HappyDesk.Infrastructure.Services;

public class LoggerService : ILoggerService
{
    public void Log(string message)
    {
        var datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        File.AppendAllText(Filenames.Trace, datetime + " >> " + message + "\n\n");
    }
}
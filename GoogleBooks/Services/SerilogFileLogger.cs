using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GoogleBooks.Services
{
    public class SerilogFileLogger : ILogger
    {
        public SerilogFileLogger()
        {
            Configure();
        }

        public void Information(string messageTemplate)
        {
            Log.Information(messageTemplate);
        }
        public void Warning<T>(string messageTemplate, T propertyValue)
        {
            Log.Warning(messageTemplate, propertyValue);
        }
        public void Error<T>(string messageTemplate, T propertyValue)
        {
            Log.Error(messageTemplate, propertyValue);  
        }
        public void Error(Exception exception, string messageTemplate)
        {
            Log.Error(exception, messageTemplate);
        }

        private void Configure()
        {
            string logFilePath = Path
                .Combine(ApplicationData.Current.LocalFolder.Path, "logs/serilogs-.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}

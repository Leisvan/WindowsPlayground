using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBooks.Services
{
    public interface ILogger
    {
        void Information(string messageTemplate);
        void Warning<T>(string messageTemplate, T propertyValue);
        void Error<T>(string messageTemplate, T propertyValue);
        void Error(Exception exception, string messageTemplate);
    }
}

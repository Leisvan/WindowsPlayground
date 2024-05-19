using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBooks.Services
{
    public class HttpTaskResult<T>
    {
        public T Result { get; set; }
        public HttpTaskResultType ResultType { get; set; }
    }

    public enum HttpTaskResultType
    {
        OK,
        Error,
        Canceled,
    }
}

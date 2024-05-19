using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleBooks.Services
{
    public interface IHttpPool
    {
        IHttpTask CreateTask();
    }
    public interface IHttpTask
    {
        Task<HttpTaskResult<T>> GetJsonAsync<T>(string url, object queryParameters, CancellationToken cancellationToken = default);
        Task<HttpTaskResult<string>> DownloadDocumentAsync(string url, CancellationToken cancellationToken = default);
        void Dispose();
    }
}

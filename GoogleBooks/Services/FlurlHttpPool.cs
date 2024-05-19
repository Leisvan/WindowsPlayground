using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace GoogleBooks.Services
{
    public class FlurlHttpPool : IHttpPool
    {
        public IHttpTask CreateTask()
        {
            return new FlurlTask();
        }
    }

    public class FlurlTask : IHttpTask
    {
        private const string DOWNLOAD_FOLDER_NAME = "Downloads";
        private const int DOWNLOAD_BUFFER_SIZE = 0x1000;
        
        #region Known Request Headers
        private const string HEADER_ACCEPT_NAME = "Accept";
        private const string HEADER_ACCEPT_VALUE = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
        private const string HEADER_UAGENT_NAME = "User-Agent";
        private const string HEADER_UAGENT_VALUE = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36 Edg/99.0.1150.39";
        #endregion

        public FlurlTask()
        {
        }

        public async Task<HttpTaskResult<T>> GetJsonAsync<T>(
            string url, 
            object queryParameters, 
            CancellationToken cancellationToken = default)
        {
            var flurlUrl = url.SetQueryParams(queryParameters);

            HttpTaskResultType resultType = HttpTaskResultType.OK;
            T result = default;
            try
            {
                result = await flurlUrl.GetJsonAsync<T>(cancellationToken);
            }
            catch (Exception e)
            {
                resultType = GetResultType(e);
            }
            return new HttpTaskResult<T>
            {
                ResultType = resultType,
                Result = result
            };
        }
        
        public async Task<HttpTaskResult<string>> DownloadDocumentAsync(
            string url, 
            CancellationToken cancellationToken = default)
        {
            string downloadsPath = Path
                .Combine(ApplicationData.Current.LocalFolder.Path, DOWNLOAD_FOLDER_NAME);

            string fileName = Path.GetFileNameWithoutExtension(url)
                + Guid.NewGuid().ToString().Split("-")[0]
                + Path.GetExtension(url);

            var flurlUrl = new Url(url)
                .WithHeader(HEADER_ACCEPT_NAME, HEADER_ACCEPT_VALUE)
                .WithHeader(HEADER_UAGENT_NAME, HEADER_UAGENT_VALUE);
            
            HttpTaskResultType resultType = HttpTaskResultType.OK;
            string result = string.Empty;
            try
            {
                result = await flurlUrl.DownloadFileAsync(downloadsPath, fileName, DOWNLOAD_BUFFER_SIZE, cancellationToken);
            }
            catch (Exception e)
            {
                resultType = GetResultType(e);
            }
            return new HttpTaskResult<string>
            {
                ResultType = resultType,
                Result = result,
            };
        }
        
        public void Dispose()
        {
        }


        private HttpTaskResultType GetResultType(Exception e)
        {
            if (e is TaskCanceledException)
            {
                return HttpTaskResultType.Canceled;
            }
            return HttpTaskResultType.Error;
        }
    }
}

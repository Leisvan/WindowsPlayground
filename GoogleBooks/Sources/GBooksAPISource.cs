using GoogleBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using GoogleBooks.Sources.JsonModels;
using GoogleBooks.Services;

namespace GoogleBooks.Sources
{
    public class GBooksAPISource : IBooksSource
    {
        private const string G_BOOKS_API_QUERYEP = "https://www.googleapis.com/books/v1/volumes";
        private readonly IHttpPool _httpPool;

        public GBooksAPISource(IHttpPool httpPool)
        {
            _httpPool = httpPool;
        }
        public async Task<List<IBook>> GetBooksAsync(string query, int maxResults = 30)
        {
            if (!string.IsNullOrEmpty(query))
            {
                if (maxResults <= 0 || maxResults > 30)
                {
                    maxResults = 30;
                }
                var httpTask = _httpPool.CreateTask();
                var result = await httpTask
                    .GetJsonAsync<GoogleVolumesJsonResult>(
                    G_BOOKS_API_QUERYEP, 
                    new { max_results = maxResults, q = query });
                if (result.ResultType == HttpTaskResultType.OK)
                {
                    return result.Result.Items.Select(item =>
                    {
                        return new Book
                        {
                            Title = item.VolumeInfo.Title,
                            Authors = item.VolumeInfo.Authors,
                            LanguageTag = item.VolumeInfo.Language,
                            Year = item.VolumeInfo.PublishedDate,
                            AvgRating = item.VolumeInfo.AverageRating ?? 0d,
                            RatingCount = item.VolumeInfo.RatingsCount ?? 0,
                            DownloadLink = item.AccessInfo.Epub.DownloadLink ?? item.AccessInfo.Pdf.DownloadLink,
                            InfoLink = item.VolumeInfo.InfoLink,
                            ReadOnlineLink = item.AccessInfo.WebReaderLink,
                            ThumbnailLink = item.VolumeInfo.ImageLinks?.Thumbnail,
                        };
                    }).ToList<IBook>();
                }

                
            }

            return Enumerable.Empty<IBook>().ToList();
        }

    }
}

using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace GoogleBooks.Services
{
    public class ImageCacheService : IImageCacheService
    {
        public async Task<BitmapImage> GetFromCacheAsync(string uri)
        {
            var imgCache = ImageCache.Instance;
            if (Uri.TryCreate(uri, UriKind.Absolute, out Uri result))
            {
                var img = await imgCache.GetFromCacheAsync(result, false);
                if (img == null)
                {
                    await imgCache.RemoveAsync(new Uri[] { result });
                }
                return img;
            }
            return null;
        }

        public async Task InitializeAsync()
        {
            var imgCache = ImageCache.Instance;
            await imgCache.InitializeAsync();
            imgCache.CacheDuration = TimeSpan.FromDays(1);
            imgCache.MaxMemoryCacheCount = 100;
            imgCache.RetryCount = 3;
        }
    }
}

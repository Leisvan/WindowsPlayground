using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace GoogleBooks.Services
{
    public interface IImageCacheService
    {
        Task InitializeAsync();
        Task<BitmapImage> GetFromCacheAsync(string uri);
    }
}

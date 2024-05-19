using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBooks.Models
{
    public interface IBook
    {
        string Title { get; set; }
        string[] Authors { get; set; }
        string ThumbnailLink { get; set; }
        string LanguageTag { get; set; }
        string Year { get; set; }
        
        double AvgRating { get; set; }
        int RatingCount { get; set; }

        string InfoLink { get; set; }
        string ReadOnlineLink { get; set; }
        string DownloadLink { get; set; }
    }
}

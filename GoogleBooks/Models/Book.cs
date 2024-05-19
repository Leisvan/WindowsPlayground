using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBooks.Models
{
    internal class Book : IBook
    {
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public string ThumbnailLink { get; set; }
        public string LanguageTag { get; set; }
        public string Year { get; set; }

        public double AvgRating { get; set; }
        public int RatingCount { get; set; }

        public string InfoLink { get; set; }
        public string ReadOnlineLink { get; set; }
        public string DownloadLink { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}

using GoogleBooks.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using Windows.UI.Xaml.Media;

namespace GoogleBooks.ViewModels
{
    public class BookViewModel: ObservableObject
    {
        private const string SUBTITLE_FORMAT = "{0} · {1}";
        private const string AUTHORS_SEPARATOR = ", ";
        private const string DATE_SEPARATOR = "-";
        private readonly IBook _model;
        private ImageSource _imgSource;

        public string Title
        {
            get => _model.Title;
        }
        
        public string ThumbnailUri
        {
            get => _model.ThumbnailLink;
        }
        public string LanguageTag
        {
            get => _model.LanguageTag;
        }
        public double AverageRating
        {
            get => _model.AvgRating;
        }
        public int RatingsCount
        {
            get => _model.RatingCount;
        }
        public string InfoLink
        {
            get => _model.InfoLink;
        }

        public string YearFormatted
        {
            get
            {
                string year = _model.Year;
                if (string.IsNullOrEmpty(year))
                {
                    return string.Empty;
                }
                return year.Split(DATE_SEPARATOR)[0];
            }
        }
        public string SubtitleFormatted
        {
            get => string.Format(SUBTITLE_FORMAT, YearFormatted, LanguageTag.ToUpperInvariant());
        }
        public string AuthorsFormmated
        {
            get
            {
                var authors = _model.Authors;
                if (authors == null)
                {
                    return String.Empty;
                }
                if (authors.Length == 1)
                {
                    return authors[0];
                }
                return string.Join(AUTHORS_SEPARATOR, authors);
            }
        }

        public ImageSource Thumbnail
        {
            get => _imgSource;
            set => SetProperty(ref _imgSource, value);
        }

        public BookViewModel(IBook book)
        {
            _model = book;
        }
    }
}

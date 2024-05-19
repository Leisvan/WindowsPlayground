using GoogleBooks.Services;
using GoogleBooks.Sources;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace GoogleBooks.ViewModels
{

    public class MainViewModel : ObservableObject, IMainViewModel
    {
        private readonly IBooksSource _booksSource;
        private readonly IImageCacheService _imgCache;
        private readonly ILogger _logger;
        private readonly IHttpPool _httpPool;
        private readonly AdvancedCollectionView _searchResults;
        private readonly LoadingViewModel _loading;
        private readonly DownloadManagerViewModel _dmanager;

        private string _searchTerm;

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                SetProperty(ref _searchTerm, value);
                UpdateBooksCollection();
            }
        }
        public ICollectionView SearchResults
        {
            get => _searchResults;
        }
        public ICommand SortCommand { get; set; }
        public LoadingViewModel Loading
        {
            get => _loading;
        }
        public DownloadManagerViewModel DownloadManager
        {
            get => _dmanager;
        }

        public MainViewModel(
            IBooksSource source, 
            ILogger logger, 
            IHttpPool httpPool,
            IImageCacheService imgCache)
        {
            _booksSource = source;
            _imgCache = imgCache;
            _logger = logger;
            _httpPool = httpPool;

            _loading = new LoadingViewModel();
            _dmanager = new DownloadManagerViewModel(_httpPool);
            _searchResults = new AdvancedCollectionView();
            SortCommand = new RelayCommand<SortCriteria>(SortAction);
        }

        private void SortAction(SortCriteria criteria)
        {
            var sortDescription = criteria switch
            {
                SortCriteria.Author => new SortDescription(nameof(BookViewModel.AuthorsFormmated), SortDirection.Ascending),
                _ => new SortDescription(nameof(BookViewModel.Title), SortDirection.Ascending),
            };
            _searchResults.SortDescriptions.Clear();
            if (sortDescription != null)
            {
                _searchResults.SortDescriptions.Add(sortDescription);
            }
        }
        private async void UpdateBooksCollection()
        {
            _logger.Information($"Querying search term: {SearchTerm}");
            _loading.ChangeState(LoadingState.Busy);
            try
            {
                _searchResults.SortDescriptions.Clear();

                var results = await _booksSource
                    .GetBooksAsync(SearchTerm);

                var vmCollection = results
                    .Select(bookModel => new BookViewModel(bookModel))
                    .ToList();

                _searchResults.Source = vmCollection;

                foreach (var item in vmCollection)
                {
                    LoadBookThumbnail(item);
                }
                _loading.ChangeState(LoadingState.Idle);
                _logger.Information($"Query succesfull for search term: {SearchTerm}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error querying data for search term: {SearchTerm}");
                _loading.ChangeState(LoadingState.Error);
            }

        }

        private async void LoadBookThumbnail(BookViewModel vm)
        {
            var thumbnail = await _imgCache.GetFromCacheAsync(vm.ThumbnailUri);
            if (thumbnail == null)
            {
                thumbnail = new BitmapImage(new Uri("ms-appx:///Assets/NoCover.png"));
            }
            vm.Thumbnail = thumbnail;
        }

    }
}

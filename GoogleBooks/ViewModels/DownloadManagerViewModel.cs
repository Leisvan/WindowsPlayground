using GoogleBooks.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Data;

namespace GoogleBooks.ViewModels
{
    public class DownloadManagerViewModel: ObservableObject
    {
        private readonly IHttpPool _httpPool;
        private string _userUrl;
        public string UserUrl
        {
            get => _userUrl;
            set => SetProperty(ref _userUrl, value);
        }
        private ObservableCollection<DownloadItemViewModel> _items;
        private ICollectionView _itemsView;

        public ICollectionView ItemsView
        {
            get => _itemsView;
        }

        public ICommand DownloadCommand 
        { 
            get; 
            set; 
        }
        public ICommand ClearAllCommand
        {
            get;
            set;
        }

        public DownloadManagerViewModel(IHttpPool httpPool)
        {
            _httpPool = httpPool;
            DownloadCommand = new RelayCommand(DownloadUrlAction);
            ClearAllCommand = new RelayCommand(ClearAllAction);
            _items = new ObservableCollection<DownloadItemViewModel>();
            _itemsView = new AdvancedCollectionView(_items);
        }

        private void ClearAllAction()
        {
            var toRemove = _items
                .Where(x => x.State != DownloadState.Downloading && x.State != DownloadState.Cancelling)
                .ToList();
            
            toRemove.ForEach(x => _itemsView.Remove(x));
        }

        private void DownloadUrlAction()
        {
            if (!string.IsNullOrWhiteSpace(UserUrl))
            {
                var item = new DownloadItemViewModel(UserUrl, _httpPool.CreateTask());
                _items.Insert(0, item);
            }
        }
    }
}

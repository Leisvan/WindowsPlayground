using GoogleBooks.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Threading;
using System.Windows.Input;
using Windows.Storage;

namespace GoogleBooks.ViewModels
{
    public class DownloadItemViewModel : ObservableObject
    {
        private readonly IHttpTask _httpTask;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private string _url;
        private DownloadState _state;
        private string _localPath;

        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }
        public DownloadState State
        {
            get => _state;    
            set => SetProperty(ref _state, value);
        }
        public ICommand CancelCommand
        {
            get;
            set;
        }
        public ICommand OpenCommand
        {
            get;
            set;
        }

        public DownloadItemViewModel(string url, IHttpTask httpTask)
        {
            _httpTask = httpTask;
            Url = url;

            _cancellationTokenSource = new CancellationTokenSource();
            CancelCommand = new RelayCommand(CancelAction);
            OpenCommand = new RelayCommand(OpenAction);
            Start();
        }

        private async void OpenAction()
        {
            if (!string.IsNullOrEmpty(_localPath))
            {
                var file = await StorageFile.GetFileFromPathAsync(_localPath);
                if (file != null)
                {
                    await Windows.System.Launcher.LaunchFileAsync(file);
                }
            }
        }

        private void CancelAction()
        {
            _cancellationTokenSource.Cancel();
        }

        private async void Start()
        {
            var token = _cancellationTokenSource.Token;
            State = DownloadState.Downloading;

            var result = await _httpTask.DownloadDocumentAsync(Url, token);
            _localPath = result.Result;
            State = result.ResultType switch
            {
                HttpTaskResultType.Canceled => DownloadState.Cancelled,
                HttpTaskResultType.Error => DownloadState.Error,
                _ => DownloadState.Finished,
            };
        }

    }
    public enum DownloadState
    {
        Downloading,
        Finished,
        Cancelling,
        Cancelled,
        Error,
    }
}

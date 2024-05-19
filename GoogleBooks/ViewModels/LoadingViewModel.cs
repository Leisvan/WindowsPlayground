using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GoogleBooks.ViewModels
{
    public class LoadingViewModel: ObservableObject
    {
        private LoadingState _state;

        public LoadingState State
        {
            get => _state;
        }

        public bool Idle => _state == LoadingState.Idle;
        public bool Busy => _state == LoadingState.Busy;
        public bool Error => _state == LoadingState.Error;

        public void ChangeState(LoadingState state)
        {
            SetProperty(ref _state, state);
            OnPropertyChanged(nameof(Error));
            OnPropertyChanged(nameof(Idle));
            OnPropertyChanged(nameof(Busy));
        }
    }

    public enum LoadingState
    {
        Idle,
        Busy,
        Error,
    }
}

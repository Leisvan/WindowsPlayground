using System.Windows.Input;
using Windows.UI.Xaml.Data;

namespace GoogleBooks.ViewModels
{
    public interface IMainViewModel
    {
        ICollectionView SearchResults { get; }
        string SearchTerm { get; set; }
        ICommand SortCommand { get; set; }
    }
}

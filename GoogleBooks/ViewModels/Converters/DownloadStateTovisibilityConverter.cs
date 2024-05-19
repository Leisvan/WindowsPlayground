using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GoogleBooks.ViewModels.Converters
{
    public class DownloadStateToVisibilityConverter : IValueConverter
    {
        public DownloadState State { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DownloadState state && State == state)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DownloadState.Error;
        }
    }
}

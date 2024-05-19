using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace GoogleBooks.ViewModels.Converters
{
    public class DownloadStateToMessageConverter: IValueConverter
    {
        private ResourceLoader _rloader = ResourceLoader.GetForViewIndependentUse();
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DownloadState state)
            {
                var resourceKey = state switch
                {
                    DownloadState.Finished => "Label-DownloadingState-Finished",
                    DownloadState.Downloading => "Label-DownloadingState-Downloading",
                    DownloadState.Cancelling => "Label-DownloadingState-Cancelling",
                    DownloadState.Cancelled => "Label-DownloadingState-Cancelled",
                    _ => "Label-DownloadingState-Error"
                };
                return _rloader.GetString(resourceKey); 
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DownloadState.Error;
        }
    }
}

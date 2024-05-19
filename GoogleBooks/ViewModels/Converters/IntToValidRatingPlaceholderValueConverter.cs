using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace GoogleBooks.ViewModels.Converters
{
    public class NumberToValidRatingPlaceholderValueConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (double.TryParse(value.ToString(), out double number))
            {
                if (number < 0.5d)
                {
                    return -1d;
                }
                return number;
            }
            return -1d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}

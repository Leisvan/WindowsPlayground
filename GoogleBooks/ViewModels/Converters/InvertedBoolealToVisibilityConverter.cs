using Microsoft.Toolkit.Uwp.UI.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace GoogleBooks.ViewModels.Converters
{
    public class InvertedBoolealToVisibilityConverter: BoolToObjectConverter
    {
        public InvertedBoolealToVisibilityConverter()
        {
            TrueValue = Visibility.Collapsed;
            FalseValue = Visibility.Visible;
        }
    }
}

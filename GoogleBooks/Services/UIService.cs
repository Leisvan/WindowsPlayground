using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;

namespace GoogleBooks.Services
{
    public static class UIService
    {
        public static void ExtendTitleBar()
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }
        public static SystemTheme GetSystemTheme()
        {
            string a = new UISettings().GetColorValue(UIColorType.Background).ToString();
            if (a == "#FF000000")
            {
                return SystemTheme.Dark;
            }

            if (a == "#FFFFFFFF")
            {
                return SystemTheme.Light;
            }

            return SystemTheme.Unknown;
        }
        public enum SystemTheme
        {
            Light,
            Dark,
            Unknown
        }
    }
}

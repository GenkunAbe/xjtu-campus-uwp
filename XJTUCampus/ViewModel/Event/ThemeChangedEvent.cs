using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace XJTUCampus.ViewModel.Event
{
    public class ThemeChangedEvent
    {
        private static readonly Color LightStatusBarColor = Color.FromArgb(255, 56, 142, 60);
        private static readonly Color DarkStatusBarColor = Color.FromArgb(255, 0, 118, 14);



        public static void Change(CurrentTheme theme)
        {
            SetStatusBarColor(theme.Theme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light);
        }

        public static void SetStatusBarColor(ElementTheme theme)
        {
            Color color = theme == ElementTheme.Light ? LightStatusBarColor : DarkStatusBarColor;


            // Setting StatusBar Color
            // For Mobile
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar sb = StatusBar.GetForCurrentView();
                sb.BackgroundColor = color;
                sb.BackgroundOpacity = 1;
            }

            // For Desktop
            ApplicationView appView = ApplicationView.GetForCurrentView();
            ApplicationViewTitleBar titleBar = appView.TitleBar;
            // TitleBar Color

            titleBar.BackgroundColor = color;
            titleBar.InactiveBackgroundColor = color;
            // Button in TitleBar
            titleBar.ButtonBackgroundColor = color;
            titleBar.ButtonHoverBackgroundColor = color;
            titleBar.ButtonPressedBackgroundColor = color;
            titleBar.ButtonInactiveBackgroundColor = color;
        }
    }
}

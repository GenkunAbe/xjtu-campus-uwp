using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace xjtu_campus_uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplashPage : Page
    {
        public SplashPage(LaunchActivatedEventArgs e)
        {
            this.InitializeComponent();
            Page_Loaded(null, e);
        }

        private async void Page_Loaded(object sender, LaunchActivatedEventArgs e)
        {

            

            // 针对mobile
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                // 需要引用Windows Mobile Extensions for the UWP
                StatusBar sb = StatusBar.GetForCurrentView();
                // 背景色设置为需要颜色
                sb.BackgroundColor = Color.FromArgb(255, 81, 157, 73);
                sb.BackgroundOpacity = 1;
            }

            // 针对desktop
            ApplicationView appView = ApplicationView.GetForCurrentView();
            ApplicationViewTitleBar titleBar = appView.TitleBar;
            // 背景色设置为需要颜色
            Color bc = Color.FromArgb(255, 81, 157, 73);
            titleBar.BackgroundColor = bc;
            titleBar.InactiveBackgroundColor = bc;
            // 按钮背景色按需进行设置
            titleBar.ButtonBackgroundColor = bc;
            titleBar.ButtonHoverBackgroundColor = bc;
            titleBar.ButtonPressedBackgroundColor = bc;
            titleBar.ButtonInactiveBackgroundColor = bc;

            // Create a Frame to act as the navigation context and navigate to the first page
            await Task.Delay(1000);



            Frame rootFrame = new Frame();
            rootFrame.Navigate(typeof(LoginPage));
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;

            
        }

        
    }
}

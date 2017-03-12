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
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XJTUCampus.Core.Model;
using XJTUCampus.ViewModel.Event;

namespace XJTUCampus.View
{

    public sealed partial class SplashPage : Page
    {
        public SplashPage(LaunchActivatedEventArgs e)
        {
            this.InitializeComponent();
            Loaded += Page_Loaded;
            //Page_Loaded(this, null);
            if (!e.PrelaunchActivated)
            {
                InsertVoiceCommands();
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {

            // Setting StatusBar Color
            ThemeChangedEvent.SetStatusBarColor(ElementTheme.Default);

            // Create a Frame to act as the navigation context and navigate to the first page
            bool isAutoLoginVaild = await Authetication.AutoLoginAutheticate();

            Frame rootFrame = new Frame();
            if (isAutoLoginVaild)
            {
                rootFrame.Navigate(typeof(MainPage));
            }
            else
            {
                await Task.Delay(1000);
                rootFrame.Navigate(typeof(LoginPage));
            }

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
    
        }

        private async void InsertVoiceCommands()
        {
            Uri uri = new Uri("ms-appx:///VoiceCommands.xml");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);
        }

    }
}

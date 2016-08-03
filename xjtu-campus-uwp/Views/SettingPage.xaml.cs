using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace xjtu_campus_uwp.Views
{
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
            GetSwitchStatus();
        }

        private async void GetSwitchStatus()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            
            try
            {
                StorageFile helloFile = await folder.GetFileAsync("hello");
                string helloStatus = await FileIO.ReadTextAsync(helloFile);
                HelloSwitch.IsOn = Boolean.Parse(helloStatus);
            }
            catch (Exception)
            {
                Debug.WriteLine("Getting Setting Failed!");
            }
        }


        private async void HelloSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile helloFile;
            try
            {
                helloFile = await folder.GetFileAsync("hello");
            }
            catch (Exception)
            {
                helloFile = await folder.CreateFileAsync("hello");
            }
            await FileIO.WriteTextAsync(helloFile, "" + HelloSwitch.IsOn);
            if (HelloSwitch.IsOn)
            {
                try
                {
                    StorageFile payFile = await folder.GetFileAsync("pay");
                    App.PayPsw = await FileIO.ReadTextAsync(payFile);
                }
                catch (Exception)
                {
                    StorageFile payFile = await folder.CreateFileAsync("pay");
                    await FileIO.WriteTextAsync(payFile, "951214");
                }
            }
        }
    }
}

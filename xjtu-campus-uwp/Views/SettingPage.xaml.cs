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

            if (HelloSwitch.IsOn)
            {
                try
                {
                    StorageFile payFile = await folder.GetFileAsync("pay");
                    App.PayPsw = await FileIO.ReadTextAsync(payFile);
                    await FileIO.WriteTextAsync(helloFile, "" + true);
                }
                catch (Exception)
                {
                    EnterPasswordPanel.Visibility = Visibility.Visible;
                    HelloLabel.Visibility = Visibility.Collapsed;
                    HelloSwitch.IsOn = false;
                }
            }
            else
            {
                await FileIO.WriteTextAsync(helloFile, "" + false);
                try
                {
                    StorageFile payFile = await folder.GetFileAsync("pay");
                    await payFile.DeleteAsync();
                }
                catch (Exception)
                {
                    Debug.WriteLine("Delete Paypsw Failed!");
                }
            }
            
        }

        private async void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile payFile;
            try
            {
                payFile = await folder.GetFileAsync("pay");
            }
            catch (Exception)
            {
                payFile = await folder.CreateFileAsync("pay");
            }
            await FileIO.WriteTextAsync(payFile, PayPasswordBox.Password);
            EnterPasswordPanel.Visibility = Visibility.Collapsed;
            HelloLabel.Visibility = Visibility.Visible;
            HelloSwitch.IsOn = true;
            PayPasswordBox.Password = "";
        }
    }
}

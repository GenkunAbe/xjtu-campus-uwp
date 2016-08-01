using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using xjtu_campus_uwp.Models;

namespace xjtu_campus_uwp
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            AutoLogin();
            this.InitializeComponent();
        }

        private async void AutoLogin()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            try
            {
                var netIdFile = await folder.GetFileAsync("netId");
                IList<string> lines = await FileIO.ReadLinesAsync(netIdFile);
                await ShowLoging(lines[0], lines[1]);
            }
            catch (Exception)
            {
                Debug.WriteLine("Auto Login Failed!");
            }
        }

        private async Task<bool> ShowLoging(string usr, string psw)
        {
            var dialog = new ContentDialog
            {
                Title = "XJTU Campus",
                Content = "Autheticating",
                IsPrimaryButtonEnabled = true
            };
#pragma warning disable CS4014
            dialog.ShowAsync();
#pragma warning restore CS4014

            bool result = await Authetication.LoginAutheticate(usr, psw);
            if (result)
            {
                dialog.Content = "Login Success!";
                App.NetId = usr;
                App.Psw = psw;
                await Task.Delay(1000);
                dialog.Hide();

                Frame rootFrame = new Frame();
                rootFrame.Navigate(typeof(MainPage));
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;

                return true;
            }
            else
            {
                dialog.Content = "Login Failed!";
                await Task.Delay(1000);
                dialog.Hide();
                return false;
            }
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool result = await ShowLoging(NetIDTextBox.Text, PasswordTextBox.Password);
            if (result)
            {
                SaveNetId(NetIDTextBox.Text, PasswordTextBox.Password);
            }
        }

        private async void SaveNetId(string netId, string psw)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile netIdFile;
            try
            {
                netIdFile = await folder.GetFileAsync("netId");
            }
            catch (Exception)
            {
                netIdFile = await folder.CreateFileAsync("netId");
            }

            await FileIO.WriteTextAsync(netIdFile, netId);
            await FileIO.AppendTextAsync(netIdFile, "\n" + psw);
        }
    }
}

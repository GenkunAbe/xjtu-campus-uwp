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
using XJTUCampus.Core.Model;

namespace XJTUCampus.View
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }    

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool result = await ShowLoging(NetIdTextBox.Text, PasswordTextBox.Password);
            if (result && PasswordSaving.IsOn)
            {
                Frame rootFrame = new Frame();
                rootFrame.Navigate(typeof(MainPage));
                Window.Current.Content = rootFrame;

                Authetication.SaveNetId(NetIdTextBox.Text, PasswordTextBox.Password);
            }
        }

        private async Task<bool> ShowLoging(string usr, string psw)
        {
            LoginProgressRing.IsActive = true;
            ResultTextBlock.Text = "Autheticating";

            bool result = await Authetication.LoginAutheticate(usr, psw);
            LoginProgressRing.IsActive = false;
            if (result)
            {
                ResultTextBlock.Text = "Login Success!";
                UserData.NetId = usr;
                UserData.Psw = psw;
                await Task.Delay(500);
                return true;
            }
            ResultTextBlock.Text = "Login Failed!";
            return false;
        }

    }
}

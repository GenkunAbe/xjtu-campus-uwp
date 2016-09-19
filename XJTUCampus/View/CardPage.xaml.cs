using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials.UI;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using XJTUCampus.Core.Model;

namespace XJTUCampus.View
{

    public sealed partial class CardPage : Page
    {
        public CardPage()
        {
            this.InitializeComponent();
            LoadCaptcha();
            GetWindowsHelloSetting();
        }

        private async void GetWindowsHelloSetting()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile helloFile = await folder.GetFileAsync("hello");
                string helloFileString = await FileIO.ReadTextAsync(helloFile);
                bool isHelloOn = Boolean.Parse(helloFileString);
                if (isHelloOn)
                {
                    PasswordLabel.Visibility = Visibility.Collapsed;
                    PasswordTextBox.Visibility = Visibility.Collapsed;
                    StorageFile payFile = await folder.GetFileAsync("pay");
                    UserData.PayPsw = await FileIO.ReadTextAsync(payFile);
                }
                else
                {
                    PasswordLabel.Visibility = Visibility.Visible;
                    PasswordTextBox.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                PasswordLabel.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Visible;
                Debug.WriteLine("Getting Setting Failed!");
            }
        }

        private async void LoadCaptcha(bool isChange = false)
        {
            ChangeCaptchaButton.Visibility = Visibility.Collapsed;
            CaptchaImg.Visibility = Visibility.Collapsed;
            LoadingCaptchaProgressRing.IsActive = true;
            CaptchaImg.Source = await (isChange ? CardManager.ChangeCaptcha() : CardManager.GetCaptcha());
            LoadingCaptchaProgressRing.IsActive = false;
            CaptchaImg.Visibility = Visibility.Visible;
            ChangeCaptchaButton.Visibility = Visibility.Visible;
        }

        private async void SubmitButton_OnClick (object sender, RoutedEventArgs e)
        {
            string rawPsw;
            if (PasswordTextBox.Visibility == Visibility.Collapsed)
            {
                UserConsentVerifierAvailability consentAvailability = await UserConsentVerifier.CheckAvailabilityAsync();
                ContentDialog dialog;
                if (consentAvailability == UserConsentVerifierAvailability.Available)
                {
                    UserConsentVerificationResult consentResult = await UserConsentVerifier.RequestVerificationAsync("Please Verify your identity.");
                    if (consentResult == UserConsentVerificationResult.Verified)
                    {
                        rawPsw = UserData.PayPsw;
                    }
                    else
                    {
                        dialog = new ContentDialog()
                        {
                            Title = "Warning",
                            Content = "UserConsentVerify Failed!",
                            PrimaryButtonText = "Ok",
                            SecondaryButtonText = "Cancel",
                            FullSizeDesired = false,
                        };
                        await dialog.ShowAsync();
                        PasswordLabel.Visibility = Visibility.Visible;
                        PasswordTextBox.Visibility = Visibility.Visible;
                        return;
                    }
                }
                else
                {
                    dialog = new ContentDialog()
                    {
                        Title = "Warning",
                        Content = "UserConsentVerifier Not Available!",
                        PrimaryButtonText = "Ok",
                        SecondaryButtonText = "Cancel",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                    PasswordLabel.Visibility = Visibility.Visible;
                    PasswordTextBox.Visibility = Visibility.Visible;
                    return;
                }
            }
            else
            {
                rawPsw = PasswordTextBox.Password;
            }
                
            string code = CodeTextBox.Text;
            string amt = AmtTextBox.Text;
            if (rawPsw == "" || code == "" || amt == "")
            {
                var dialog = new ContentDialog()
                {
                    Title = "Warning",
                    Content = "Please enter all items of this form!",
                    PrimaryButtonText = "Ok",
                    SecondaryButtonText = "Cancel",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
                PasswordLabel.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Visible;
                return;
            }
            PayResult result = await CardManager.Pay(rawPsw, code, amt);

            LoadCaptcha(true);
            if (!result.ret)
            {
                CodeTextBox.Text = "";
            }
            ResultTextBlock.Text = result.msg;
        }

        private void ButtonCaptcha_OnClick(object sender, RoutedEventArgs e)
        {
            LoadCaptcha(true);
        }
    }
}

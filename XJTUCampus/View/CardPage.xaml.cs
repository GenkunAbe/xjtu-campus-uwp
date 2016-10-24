using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
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
            InitializeComponent();
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

        private async void SubmitButton_OnClick (object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = "";
            string rawPsw;
            // There is no PasswordTextBox, so do Windows Hello User Authentication
            if (PasswordTextBox.Visibility == Visibility.Collapsed)
            {
                UserConsentVerifierAvailability consentAvailability = await UserConsentVerifier.CheckAvailabilityAsync();
                if (consentAvailability == UserConsentVerifierAvailability.Available)
                {
                    UserConsentVerificationResult consentResult = await UserConsentVerifier.RequestVerificationAsync("Please Verify your identity.");
                    if (consentResult == UserConsentVerificationResult.Verified)
                    {
                        rawPsw = UserData.PayPsw;
                    }
                    else
                    {
                        await ShowContentDialog("Warning", "UserConsentVerify Failed!");
                        PasswordLabel.Visibility = Visibility.Visible;
                        PasswordTextBox.Visibility = Visibility.Visible;
                        return;
                    }
                }
                else
                {
                    await ShowContentDialog("Warning", "UserConsentVerifier Not Available!");
                    PasswordLabel.Visibility = Visibility.Visible;
                    PasswordTextBox.Visibility = Visibility.Visible;
                    return;
                }
            }
            else
            {
                rawPsw = PasswordTextBox.Password;
            }
                
            string amt = AmtTextBox.Text;
            if (rawPsw == "" || amt == "")
            {
                await ShowContentDialog("Warning", "Please enter all items of this form!");
                PasswordLabel.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Visible;
                return;
            }
            SubmitButton.IsEnabled = false;
            PayProcessingRing.IsActive = true;
            PayResult result = await CardManager.Pay(rawPsw, amt);
            PayProcessingRing.IsActive = false;
            SubmitButton.IsEnabled = true;
            ResultTextBlock.Text = result.msg;
        }

        private static async Task<bool> ShowContentDialog(String title, String content)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel",
                FullSizeDesired = false,
            };
            await dialog.ShowAsync();
            return true;
        }
    }
}

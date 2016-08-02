using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using xjtu_campus_uwp.Models;

namespace xjtu_campus_uwp.Views
{

    public sealed partial class CardPage : Page
    {
        public CardPage()
        {
            this.InitializeComponent();
            LoadCaptcha();
        }

        private async void LoadCaptcha()
        {
            CaptchaImg.Source = await CardManager.GetCaptcha();
        }

        private async void SubmitButton_OnClick (object sender, RoutedEventArgs e)
        {
            string rawPsw = PasswordTextBox.Password;
            string code = CodeTextBox.Text;
            string amt = AmtTextBox.Text;
            string result = await CardManager.Pay(rawPsw, code, amt);

            ResultTextBlock.Text = result;
        }

        private void Captcha_Tapped (object sender, TappedRoutedEventArgs e)
        {
            LoadCaptcha();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadCaptcha();
        }

    }
}

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
            string uri = "http://202.117.14.143:12000/cardpre?usr=genkunabe&psw=Lyx@xjtu120";
            BitmapImage bitmap = await HttpHelper.GetImage(uri);
            CaptchaImg.Source = bitmap;
        }

        private void SubmitButton_OnClick (object sender, RoutedEventArgs e)
        {
            string raw_psw = PasswordTextBox.Password;
            string amt = AmtTextBox.Text;
            string code = CodeTextBox.Text;
            string uri = "http://202.117.14.143:12000/cardpost?usr=genkunabe&psw=Lyx@xjtu120&rawpsw=" + raw_psw +
                         "&code=" + code + "&amt=" + amt;
            Pay(uri);
        }

        private async void Pay (string uri)
        {
            string result = await HttpHelper.GetResponse(uri);
            ResultTextBlock.Text = result;
        }

        private void Captcha_Tapped (object sender, TappedRoutedEventArgs e)
        {
            LoadCaptcha();
        }
    }
}

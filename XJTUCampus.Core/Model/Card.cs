using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace XJTUCampus.Core.Model
{
    public class Card
    {

    }

    public class PayResult
    {
        public bool ret { get; set; }
        public string msg { get; set; }
    }

    public class CardInfo
    {
        public string loss { get; set; }
        public string balance { get; set; }
        public string temp { get; set; }
        public string freeze { get; set; }
    }

    public class CardManager
    {
        public static async Task<BitmapImage> GetCaptcha()
        {
            string uri = UserData.Host + "cardpre?usr=" + UserData.NetId + "&psw=" + UserData.Psw;
            BitmapImage bitmap = await HttpHelper.GetImage(uri);
            return bitmap;
        }

        public static async Task<BitmapImage> ChangeCaptcha()
        {
            string uri = UserData.Host + "cardchange?usr=" + UserData.NetId + "&psw=" + UserData.Psw;
            BitmapImage bitmap = await HttpHelper.GetImage(uri);
            return bitmap;
        }

        public static async Task<PayResult> Pay(string rawPsw, string code, string amt)
        {
            string uri = UserData.Host + "cardpost?usr=" + UserData.NetId + "&psw=" + UserData.Psw + "&rawpsw=" + rawPsw +
                         "&code=" + code + "&amt=" + amt;
            string resultString = await HttpHelper.GetResponse(uri);
            var result = JsonConvert.DeserializeObject<PayResult>(resultString);
            return result;
        }

        public static async Task<CardInfo> GetCardInfo()
        {
            string uri = UserData.Host + "cardinfo?usr=" + UserData.NetId + "&psw=" + UserData.Psw;
            string resultString = await HttpHelper.GetResponse(uri);
            var result = JsonConvert.DeserializeObject<CardInfo>(resultString);
            return result;
        }

        public static async Task<string> GetCardInfoString()
        {
            string uri = UserData.Host + "cardinfo?usr=" + UserData.NetId + "&psw=" + UserData.Psw;
            string result = await HttpHelper.GetResponse(uri);
            return result;
        }

    }
}

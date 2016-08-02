using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace xjtu_campus_uwp.Models
{
    public class Card
    {

    }

    public class CardManager
    {
        public static async Task<BitmapImage> GetCaptcha()
        {
            string uri = App.Host + "cardpre?usr=" + App.NetId + "&psw=" + App.Psw;
            BitmapImage bitmap = await HttpHelper.GetImage(uri);
            return bitmap;
        }

        public static async Task<string> Pay(string rawPsw, string code, string amt)
        {
            string uri = App.Host + "cardpost?usr=" + App.NetId + "&psw=" + App.Psw + "&rawpsw=" + rawPsw +
                         "&code=" + code + "&amt=" + amt;
            string result = await HttpHelper.GetResponse(uri);
            return result;
        }
    }
}

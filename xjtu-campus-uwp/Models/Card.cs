using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace xjtu_campus_uwp.Models
{
    public class Card
    {

    }

    [DataContract]
    public class PayResult
    {
        [DataMember]
        public bool ret { get; set; }
        [DataMember]
        public string msg { get; set; }
    }

    [DataContract]
    public class CardInfo
    {
        [DataMember]
        public string loss { get; set; }
        [DataMember]
        public string balance { get; set; }
        [DataMember]
        public string temp { get; set; }
        [DataMember]
        public string freeze { get; set; }
    }




    public class CardManager
    {
        public static async Task<BitmapImage> GetCaptcha()
        {
            string uri = App.Host + "cardpre?usr=" + App.NetId + "&psw=" + App.Psw;
            BitmapImage bitmap = await HttpHelper.GetImage(uri);
            return bitmap;
        }

        public static async Task<PayResult> Pay(string rawPsw, string code, string amt)
        {
            string uri = App.Host + "cardpost?usr=" + App.NetId + "&psw=" + App.Psw + "&rawpsw=" + rawPsw +
                         "&code=" + code + "&amt=" + amt;
            string resultString = await HttpHelper.GetResponse(uri);
            var serializer = new DataContractJsonSerializer(typeof(PayResult));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(resultString));
            var result = (PayResult) serializer.ReadObject(ms);
            return result;
        }

        public static async Task<CardInfo> GetCardInfo()
        {
            string uri = App.Host + "cardinfo?usr=" + App.NetId + "&psw=" + App.Psw;
            string resultString = await HttpHelper.GetResponse(uri);
            var serializer = new DataContractJsonSerializer(typeof(CardInfo));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(resultString));
            var result = (CardInfo) serializer.ReadObject(ms);
            return result;
        }

    }
}

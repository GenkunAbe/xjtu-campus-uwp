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

        public PayResult(bool ret, string msg)
        {
            this.ret = ret;
            this.msg = msg;
        }
    }

    public class CardInfo
    {
        public string loss { get; set; }
        public string balance { get; set; }
        public string temp { get; set; }
        public string freeze { get; set; }

        public CardInfo()
        {
            loss = balance = temp = freeze = "-1";
        }
    }

    public class CardManager
    {
        public static async Task<PayResult> Pay(string payPsw, string amt)
        {
            payPsw = Convert.ToBase64String(Encoding.UTF8.GetBytes(payPsw));
            string uri = UserData.Host + "cardpay?usr=" + UserData.NetId + "&psw=" 
                + UserData.Psw + "&paypsw=" + payPsw + "&amt=" + amt;
            PayResult result;
            try
            {
                string resultString = await HttpHelper.GetResponse(uri);
                result = JsonConvert.DeserializeObject<PayResult>(resultString);
            }
            catch (Exception)
            {
                result = new PayResult(false, "未知错误");
            }
            return result;
        }

        public static async Task<CardInfo> GetCardInfo()
        {
            string uri = UserData.Host + "cardinfo?usr=" + UserData.NetId + "&psw=" + UserData.Psw;
            try
            {
                string resultString = await HttpHelper.GetResponse(uri);
                CardInfo result = JsonConvert.DeserializeObject<CardInfo>(resultString);
                return result;
            }
            catch (Exception)
            {
                return new CardInfo();
            }
        }

    }
}

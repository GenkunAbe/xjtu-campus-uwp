using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml;

namespace xjtu_campus_uwp.Models
{
    class Authetication
    {

        public static async Task<bool> LoginAutheticate(string netId, string psw)
        {
            string uri = "http://202.117.14.143:12000/auth?usr=" + netId + "&psw=" + psw;
            string result = await HttpHelper.GetResponse(uri);
            JsonArray arr = JsonArray.Parse(result);
            string auth = arr[0].GetString();
            return auth == "True";
        }

        
    }


}

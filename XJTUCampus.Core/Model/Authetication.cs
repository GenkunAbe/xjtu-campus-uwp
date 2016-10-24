using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml;

namespace XJTUCampus.Core.Model
{
    public class Authetication
    {

        public static async Task<bool> LoginAutheticate(string netId, string psw)
        {
            string uri = UserData.Host + "auth?usr=" + netId + "&psw=" + psw;
            string result;
            try
            {
                result = await HttpHelper.GetResponse(uri);
            }
            catch (Exception)
            {
                return false;
            }
            JsonArray arr = JsonArray.Parse(result);
            return arr[0].GetString() == "True";
        }

        public static async Task<bool> AutoLoginAutheticate()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile netIdFile = await folder.GetFileAsync("netId");
                IList<string> lines = await FileIO.ReadLinesAsync(netIdFile);
                bool authResult = await LoginAutheticate(lines[0], lines[1]);
                if (authResult)
                {
                    UserData.NetId = lines[0];
                    UserData.Psw = lines[1];
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public static async void SaveNetId(string usr, string psw)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile netIdFile;
            try
            {
                netIdFile = await folder.GetFileAsync("netId");
            }
            catch (Exception)
            {
                netIdFile = await folder.CreateFileAsync("netId");
            }

            await FileIO.WriteTextAsync(netIdFile, usr);
            await FileIO.AppendTextAsync(netIdFile, "\n" + psw);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace xjtu_campus_uwp.Models
{
    class HttpHelper
    {
        public static async Task<string> GetResponse(string url)
        {
            string result = "";

            HttpClient httpClient = new HttpClient();
            Uri uri = new Uri(url);
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                response = await httpClient.GetAsync(uri);
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                result = "Error!";
            }

            return result;
        }
    }
}

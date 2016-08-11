using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace XJTUCampus.Core.Model
{
    public class HttpHelper
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

        public static async Task<BitmapImage> GetImage(string url)
        {
            BitmapImage bitmap = new BitmapImage();
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(url));
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await response.Content.WriteToStreamAsync(stream);
                    stream.Seek(0ul);
                    bitmap.SetSource(stream);
                }

            }
            catch (Exception)
            {
                Debug.WriteLine("Get Image Error!");
            }
            return bitmap;
        }
    }
}

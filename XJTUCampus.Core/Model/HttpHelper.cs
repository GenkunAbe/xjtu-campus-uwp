using System;
using System.Diagnostics;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Security.Cryptography.Certificates;


namespace XJTUCampus.Core.Model
{
    public class HttpHelper
    {

        public static async Task<string> GetResponse(string url)

        {
            string retVal = "";
            HttpResponseMessage response = await GetHttpsResponse(url);
            retVal = await response.Content.ReadAsStringAsync();
            return retVal;
        }

        public static async Task<BitmapImage> GetImage(string url)
        {
            BitmapImage bitmap = new BitmapImage();
            HttpResponseMessage response = await GetHttpsResponse(url);
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await response.Content.WriteToStreamAsync(stream);
                stream.Seek(0ul);
                bitmap.SetSource(stream);
            }
            return bitmap;
        }

        private static async Task<HttpResponseMessage> GetHttpsResponse(string url)
        {
            Uri theUri = new Uri(url);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, theUri);
            HttpBaseProtocolFilter httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            httpBaseProtocolFilter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            httpBaseProtocolFilter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            httpBaseProtocolFilter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);

            HttpResponseMessage resp = null;
            try
            {
                HttpClient httpClient = new HttpClient(httpBaseProtocolFilter);
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, theUri);
                resp = await httpClient.SendRequestAsync(req);
            }
            catch (Exception ex2)
            {
                Debug.WriteLine(ex2.Message);
            }
            return resp;
        }

    }
}

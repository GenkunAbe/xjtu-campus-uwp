using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace xjtu_campus_uwp.Models
{
    class News
    {
    }

    class NewsGlance
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }

        public NewsGlance (IJsonValue item)
        {
            JsonArray values = JsonArray.Parse(item.ToString());
            Link = values[0].GetString();
            Title = values[1].GetString();
            Date = values[2].GetString();
        }
    }

    class NewsManager
    {
        public static async Task<ObservableCollection<NewsGlance>> GetNewsList()
        {
            string uri = "http://202.117.14.143:12000/news";
            string result = await HttpHelper.GetResponse(uri);

            JsonArray lines = JsonArray.Parse(result);
            ObservableCollection<NewsGlance> NewsList = new ObservableCollection<NewsGlance>();
            foreach (IJsonValue line in lines)
            {
                NewsList.Add(new NewsGlance(line));
            }
            return NewsList;
        }
    }
}

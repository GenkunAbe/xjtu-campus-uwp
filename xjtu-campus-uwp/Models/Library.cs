using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace xjtu_campus_uwp.Models
{

    class Library
    {
        
    }

    class BookGlance
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public BookGlance(string name, string link)
        {
            Name = name;
            Link = link;
        }
    }



    class LibraryManager
    {
        public static async Task<ObservableCollection<BookGlance>> GetBookGlanceList(string arg)
        {
            string uri = "http://192.168.0.103:12000/booksearch?arg=" + arg;
            string result = await HttpHelper.GetResponse(uri);

            ObservableCollection<BookGlance> list = new ObservableCollection<BookGlance>();
            JsonArray lines = JsonArray.Parse(result);
            foreach (IJsonValue line in lines)
            {
                JsonArray items = JsonArray.Parse(line.ToString());
                BookGlance book = new BookGlance(items[0].GetString(), items[1].GetString());
                list.Add(book);
            }
            return list;
        }
    }
}

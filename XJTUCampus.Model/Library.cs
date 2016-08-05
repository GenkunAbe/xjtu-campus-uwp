using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace XJTUCampus.Model
{

    public class Library
    {
        
    }

    public class BookGlance
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public BookGlance(string link, string name)
        {
            Link = link;
            Name = name;
        }
    }

    public class BookStatus
    {
        public string Place { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }

        public BookStatus(JsonArray array)
        {
            Place = array[0].GetString();
            Id = array[1].GetString();
            Status = array[2].GetString();
        }
    }

    public class BookDetail
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public ObservableCollection<BookStatus> Status { get; set; }

        public BookDetail(JsonArray array)
        {
            Name = array[0].GetString();
            Author = array[1].GetString();
            Press = array[2].GetString();
            JsonArray status = JsonArray.Parse(array[3].ToString());

            Status = new ObservableCollection<BookStatus>();
            foreach (IJsonValue value in status)
            {
                JsonArray line = JsonArray.Parse(value.ToString());
                Status.Add(new BookStatus(line));
            }
        }
    }

    public class LibraryManager
    {
        public static async Task<ObservableCollection<BookGlance>> GetBookGlanceList(string arg)
        {
            string uri = UserData.Host + "booksearch?arg=" + arg;
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

        public static async Task<ObservableCollection<BookDetail>> GetBookDetail(string link)
        {
            string uri = "http://202.117.14.143:12000/bookdetail?link=" + link;
            string result = await HttpHelper.GetResponse(uri);

            ObservableCollection<BookDetail> Details = new ObservableCollection<BookDetail>();
            JsonArray lines = JsonArray.Parse(result);
            // 待解决: result可能为空
            foreach (IJsonValue line in lines)
            {
                JsonArray items = JsonArray.Parse(line.ToString());
                Details.Add(new BookDetail(items));
            }
            return Details;
        }
    }
}

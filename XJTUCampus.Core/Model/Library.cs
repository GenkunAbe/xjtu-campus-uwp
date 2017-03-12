using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Data.Json;

namespace XJTUCampus.Core.Model
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

        public BookDetail(bool isInitError)
        {
            Name = isInitError ? "未知错误！" : "返回 0 条结果。";
            Author = Press = "";
            Status = new ObservableCollection<BookStatus>();
        }
    }

    public class LibraryManager
    {
        public static async Task<ObservableCollection<BookGlance>> GetBookGlanceList(string arg)
        {
            string uri = UserData.Host + "booksearch?arg=" + arg;
            ObservableCollection<BookGlance> list = new ObservableCollection<BookGlance>();
            try
            {
                string result = await HttpHelper.GetResponse(uri);
                JsonArray lines = JsonArray.Parse(result);
                foreach (IJsonValue line in lines)
                {
                    JsonArray items = JsonArray.Parse(line.ToString());
                    BookGlance book = new BookGlance(items[0].GetString(), items[1].GetString());
                    list.Add(book);
                }
            }
            catch (Exception)
            {
                list.Add(new BookGlance("", "搜索遇到问题"));
            }
            return list;
        }

        public static async Task<ObservableCollection<BookDetail>> GetBookDetail(string link)
        {
            string uri = "http://202.117.14.143:12000/bookdetail?link=" + link;
            ObservableCollection<BookDetail> Details = new ObservableCollection<BookDetail>();
            try
            {
                string result = await HttpHelper.GetResponse(uri);
                JsonArray lines = JsonArray.Parse(result);

                // if the list of response is not empty
                if (lines.Any())
                {
                    foreach (IJsonValue line in lines)
                    {
                        JsonArray items = JsonArray.Parse(line.ToString());
                        Details.Add(new BookDetail(items));
                    }
                }
                else
                {
                    Details.Add(new BookDetail(false));
                }
            }
            catch (Exception)
            {
                Details.Add(new BookDetail(true));
            }
            return Details;
        }

        public static async Task<List<VoiceCommandContentTile>> GetBookGlanceListForCortana(string arg)
        {
            ObservableCollection<BookGlance> rawList = await GetBookGlanceList(arg);
            List<VoiceCommandContentTile> retList = new List<VoiceCommandContentTile>();
            int n = rawList.Count > 10 ? 10 : rawList.Count;
            for (int i = 0; i < n; ++i)
            {
                string name = rawList[i].Name;
                if (name.Length > 98) name = name.Remove(98);
                name += $" {i}";
                retList.Add(new VoiceCommandContentTile
                {
                    AppContext = rawList[i].Link,
                    ContentTileType = VoiceCommandContentTileType.TitleOnly,
                    Title = name
                });
            }
            return retList;
        }

        public static async Task<List<VoiceCommandContentTile>> GetBookDetailForCortana(string link)
        {
            ObservableCollection<BookDetail> rawList = await GetBookDetail(link);
            int n = rawList.Count > 10 ? 10 : rawList.Count;
            List<VoiceCommandContentTile> retList = new List<VoiceCommandContentTile>();
            for (int i = 0; i < n; ++i)
            {
                var detail = rawList[i];

                var name = detail.Name.Length > 98 ? detail.Name.Remove(98) : detail.Name;
                name += $"{i}";

                var author = detail.Author + ", " + detail.Press;
                if (author.Length > 99) author = author.Remove(99);

                var bookStatus = detail.Status;
                var status = bookStatus[0].Place + "   " + bookStatus[0].Id + "\n";
                foreach (var bookStatuse in bookStatus)
                    status += bookStatuse.Status + "\n";
                if (status.Length > 99) status = status.Remove(99);

                retList.Add(new VoiceCommandContentTile
                {
                    AppContext = i,
                    ContentTileType = VoiceCommandContentTileType.TitleWithText,
                    Title = name,
                    TextLine1 = author,
                    TextLine2 = status
                });
            }
            if (retList.Count == 1)
            {
                retList.Add(new VoiceCommandContentTile
                {
                    AppContext = -1,
                    ContentTileType = VoiceCommandContentTileType.TitleWithText,
                    Title = "点击返回",
                    TextLine1 = "",
                    TextLine2 = ""
                });
            }
            return retList;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace XJTUCampus.Core.Model
{
    public class News
    {
    }

    public class NewsGlance
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }

        public NewsGlance(string link, string title, string date)
        {
            Link = link;
            Title = title;
            Date = date;
        }

        public NewsGlance (IJsonValue item)
        {
            JsonArray values = JsonArray.Parse(item.ToString());
            Link = values[1].GetString();
            Title = values[2].GetString();
            Date = values[0].GetString();
        }
    }

    public class NewsManager
    {
        private string _rawNews;
        private ObservableCollection<NewsGlance> NewsList;

        public NewsManager()
        {
            _rawNews = "";
            NewsList = new ObservableCollection<NewsGlance>();
        }

        public async Task<ObservableCollection<NewsGlance>> GetStoredNewsList()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile newsFile = await folder.GetFileAsync("news");
                _rawNews = await FileIO.ReadTextAsync(newsFile);
                JsonNewsParser();
                return NewsList;
            }
            catch (Exception)
            {
                return new ObservableCollection<NewsGlance>();
            }
        }

        public async Task<ObservableCollection<NewsGlance>> GetNewNewsList()
        {
            string uri = UserData.Host + "news?";
            try
            {
                _rawNews = await HttpHelper.GetResponse(uri);
            }
            catch (Exception)
            {
                _rawNews = "[-1]";
            }
            NewsList.Clear();
            JsonNewsParser();
            Save();
            return NewsList;
        }

        private async void Save()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile newsFile;
            try
            {
                newsFile = await folder.GetFileAsync("news");
            }
            catch (Exception)
            {
                newsFile = await folder.CreateFileAsync("news");
            }
            await FileIO.WriteTextAsync(newsFile, _rawNews);
        }

        private void JsonNewsParser()
        {
            if (_rawNews == "[-1]")
            {
                NewsList.Add(new NewsGlance("", "获取新闻遇到错误！", ""));
            }
            else
            {
                JsonArray lines = JsonArray.Parse(_rawNews);
                foreach (IJsonValue line in lines)
                {
                    NewsList.Add(new NewsGlance(line));
                }
            }
        }
    }
}

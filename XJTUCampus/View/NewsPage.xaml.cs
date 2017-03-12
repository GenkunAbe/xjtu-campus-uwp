using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XJTUCampus.Core.Model;

namespace XJTUCampus.View
{
    public sealed partial class NewsPage : Page
    {
        public static NewsPage This;
        private static readonly NewsManager NewsManager = new NewsManager();

        private readonly ObservableCollection<NewsGlance> _newsList;
        

        public NewsPage()
        {
            InitializeComponent();
            This = this;
            _newsList = new ObservableCollection<NewsGlance>();

            if (UserData.News == null || UserData.News.Count == 0)
            {
                GetStoredNewsList();
                UserData.News = _newsList;
            }
            else
            {
                UpdateNewsList(UserData.News);
            }
            
        }

        private async void GetStoredNewsList()
        {
            UpdateNewsList(await NewsManager.GetStoredNewsList());
            if (_newsList.Count == 0)
                GetNewNewsList();
        }

        private async void GetNewNewsList()
        {
            UpdateNewsList(await NewsManager.GetNewNewsList());
        }

        private void UpdateNewsList(ObservableCollection<NewsGlance> list)
        {
            _newsList.Clear();
            foreach (NewsGlance glance in list)
            {
                _newsList.Add(glance);
            }
        }

        private async void NewsListItem_OnClick(object sender, ItemClickEventArgs e)
        {
            NewsGlance news = (NewsGlance) e.ClickedItem;
            string link = news?.Link;
            if (string.IsNullOrEmpty(link))
            {
                return;
            }
            if (!link.StartsWith("http"))
                link = "http://dean.xjtu.edu.cn/" + link;
            Uri uri = new Uri(link);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        public static void Refresh()
        {
            This.GetNewNewsList();
        }

    }
}

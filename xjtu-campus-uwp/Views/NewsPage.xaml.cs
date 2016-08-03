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
using xjtu_campus_uwp.Models;

namespace xjtu_campus_uwp.Views
{
    public class News
    {
        public static ObservableCollection<NewsGlance> GlanceList { get; set; } = new ObservableCollection<NewsGlance>();
    }

    public sealed partial class NewsPage : Page
    {
        private ObservableCollection<NewsGlance> NewsList
        {
            get { return News.GlanceList; }
            set
            {
                News.GlanceList.Clear();
                foreach (NewsGlance newsGlance in value)
                {
                    News.GlanceList.Add(newsGlance);
                }
            }
        }
        private static NewsManager _NewsManager = new NewsManager();

        public NewsPage()
        {
            this.InitializeComponent();
            NewsList = News.GlanceList;
            GetStoredNewsList();
        }

        private async void GetStoredNewsList()
        {
            NewsList = await _NewsManager.GetStoredNewsList();
            if (NewsList.Count == 0)
                Refresh();
        }

        private async void NewsListItem_OnClick(object sender, ItemClickEventArgs e)
        {
            NewsGlance news = (NewsGlance) e.ClickedItem;
            string link = news.Link;
            if (!link.StartsWith("http"))
                link = "http://dean.xjtu.edu.cn" + link;
            Uri uri = new Uri(link);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        public static async void Refresh()
        {
            var tmp = await _NewsManager.GetNewNewsList();

            News.GlanceList.Clear();
            foreach (NewsGlance newsGlance in tmp)
            {
                News.GlanceList.Add(newsGlance);
            }
        }

    }
}

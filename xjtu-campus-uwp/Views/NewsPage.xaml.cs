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
    public sealed partial class NewsPage : Page
    {
        private ObservableCollection<NewsGlance> NewsList;
        public NewsPage()
        {
            this.InitializeComponent();
            GetNewsList();
        }

        private async void GetNewsList()
        {
            NewsList = await NewsManager.GetNewsList();
            NewsGlanceListView.ItemsSource = NewsList;
        }

        private void NewsListItem_OnClick(object sender, ItemClickEventArgs e)
        {
            NewsGlance news = (NewsGlance) e.ClickedItem;
            string link = news.Link;
            if (!link.StartsWith("http"))
                link = "http://dean.xjtu.edu.cn" + link;
            OpenUri(link);
        }

        private async void OpenUri(string link)
        {
            Uri uri = new Uri(link);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}

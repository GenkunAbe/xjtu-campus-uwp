using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class HomePage : Page
    {
        public ObservableCollection<NewsGlance> NewsList = new ObservableCollection<NewsGlance>();
        private NewsManager manager = new NewsManager();
        public HomePage()
        {
            this.InitializeComponent();
            GetNewsGlanceList();
            GetCardInfo();
        }

        private async void GetNewsGlanceList()
        {
            var tmp = await manager.GetStoredNewsList();
            NewsList.Clear();
            foreach (NewsGlance glance in tmp)
            {
                NewsList.Add(glance);
            }
        }

        private async void GetCardInfo()
        {
            CardInfo info = await CardManager.GetCardInfo();
            BalanceTextBlock.Text = info.balance;
        }
    }
}

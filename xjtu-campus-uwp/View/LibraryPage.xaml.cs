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
using XJTUCampus.Model;

namespace xjtu_campus_uwp.View
{

    public sealed partial class LibraryPage : Page
    {
        private ObservableCollection<BookGlance> BookList;
        public LibraryPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

        }

        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (Frame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private async void GetGlanceList(string arg)
        {
            BookList = await LibraryManager.GetBookGlanceList(arg);
            BookGlanceListView.ItemsSource = BookList;
        }

        

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetGlanceList(SearchTextBlock.Text);
        }

        private void BookGlanceItem_OnClick(object sender, ItemClickEventArgs e)
        {
            BookGlance book = (BookGlance) e.ClickedItem;
            Frame.Navigate(typeof(View.BookDetailPage), book.Link);
        }
    }
}

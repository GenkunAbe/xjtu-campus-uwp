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

    public sealed partial class LibraryPage : Page
    {
        private ObservableCollection<BookGlance> BookList;
        public LibraryPage()
        {
            this.InitializeComponent();
        }

        private async void GetGlanceList()
        {
            BookList = await LibraryManager.GetBookGlanceList(SearchTextBlock.Text);
        }

        private void SearchTextBlock_GetFocus(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetGlanceList();
        }
    }
}

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

    public sealed partial class BookDetailPage : Page
    {
        private ObservableCollection<BookDetail> Books;
        public BookDetailPage()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
            string link = (string) e.Parameter;
            GetDetail(link);
            
        }

        private async void GetDetail(string link)
        {
            Books = await LibraryManager.GetBookDetail(link);
            BookDetailList.ItemsSource = Books;
        }

    }
}

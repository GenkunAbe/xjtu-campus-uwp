using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using XJTUCampus.Model;

namespace xjtu_campus_uwp.View
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

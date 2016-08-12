using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using XJTUCampus.Core.Model;

namespace XJTUCampus.View
{

    public sealed partial class BookDetailPage : Page
    {
        public static BookDetailPage Current;
        private ObservableCollection<BookDetail> Books = new ObservableCollection<BookDetail>();
        public BookDetailPage()
        {
            this.InitializeComponent();
            Current = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string link = (string)e.Parameter;
            GetDetail(link);
        }

        private async void GetDetail(string link)
        {
            SetBooks(await LibraryManager.GetBookDetail(link));
        }

        private void SetBooks (ObservableCollection<BookDetail> books)
        {
            Books.Clear();
            foreach (BookDetail detail in books)
                Books.Add(detail);
        }

    }
}

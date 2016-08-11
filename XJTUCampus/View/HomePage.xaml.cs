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
using XJTUCampus.Core.Model;

namespace XJTUCampus.View
{
    public sealed partial class HomePage : Page
    {
        public ObservableCollection<NewsGlance> NewsList = new ObservableCollection<NewsGlance>();
        public ObservableCollection<Grade> GradeList = new ObservableCollection<Grade>();
        private NewsManager newsManager = new NewsManager();
        private GradeManager gradeManager = new GradeManager();
        private TableManager tableManager = new TableManager();
        //private NewsManager newsManager = new NewsManager();
        public HomePage()
        {
            this.InitializeComponent();
            GetNewsGlanceList();
            GetGradeList();
            GetCardInfo();
            GetTable();
        }


        private async void GetNewsGlanceList()
        {
            var tmp = await newsManager.GetStoredNewsList();
            if (NewsList.Count == 0)
                tmp = await newsManager.GetNewNewsList();
            NewsList.Clear();
            foreach (NewsGlance glance in tmp)
            {
                NewsList.Add(glance);
            }
            
        }

        private async void GetGradeList()
        {
            var tmp = await gradeManager.GetStoredGrades();
            if (NewsList.Count == 0)
                tmp = await gradeManager.GetNewGrades();
            GradeList.Clear();
            foreach (Grade grade in tmp)
            {
                GradeList.Add(grade);
            }
        }

        private async void GetTable()
        {
            ObservableCollection<Course> courses = await tableManager.GetTodayCourse();
            FirCourse.Text = courses[0].Name == "" ? "Free" : courses[0].Name;
            SecCourse.Text = courses[1].Name == "" ? "Free" : courses[1].Name;
            ThiCourse.Text = courses[2].Name == "" ? "Free" : courses[2].Name;
            FouCourse.Text = courses[3].Name == "" ? "Free" : courses[3].Name;
        }

        private async void GetCardInfo()
        {
            CardInfo info = await CardManager.GetCardInfo();
            BalanceTextBlock.Text = info.balance;
//            BalanceTextBlock.Text = await CardManager.GetCardInfoString();
        }
    }
}

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
        public static HomePage This;

        public ObservableCollection<NewsGlance> NewsList = new ObservableCollection<NewsGlance>();
        public ObservableCollection<Grade> GradeList = new ObservableCollection<Grade>();

        private readonly NewsManager _newsManager = new NewsManager();
        private readonly GradeManager _gradeManager = new GradeManager();
        private readonly TableManager _tableManager = new TableManager();
        public HomePage()
        {
            InitializeComponent();
            This = this;
            GetNewsGlanceList();
            GetGradeList();
            GetCardInfo();
            GetTable();
        }


        private async void GetNewsGlanceList()
        {
            ObservableCollection<NewsGlance> tmp = await _newsManager.GetStoredNewsList();
            if (tmp.Count == 0)
                tmp = await _newsManager.GetNewNewsList();
            NewsList.Clear();
            foreach (NewsGlance glance in tmp)
            {
                NewsList.Add(glance);
            }
            
        }

        private async void GetGradeList()
        {
            ObservableCollection<Grade> tmp = await _gradeManager.GetStoredGrades();
            if (tmp.Count == 0)
                tmp = await _gradeManager.GetNewGrades();
            GradeList.Clear();
            foreach (Grade grade in tmp)
            {
                GradeList.Add(grade);
            }
        }

        private async void GetTable()
        {
            ObservableCollection<Course> courses = await _tableManager.GetTodayCourse();
            FirCourse.Text = courses[0].Name == "" ? "Free" : courses[0].Name;
            SecCourse.Text = courses[1].Name == "" ? "Free" : courses[1].Name;
            ThiCourse.Text = courses[2].Name == "" ? "Free" : courses[2].Name;
            FouCourse.Text = courses[3].Name == "" ? "Free" : courses[3].Name;
        }

        private async void GetCardInfo()
        {
            CardInfo info = await CardManager.GetCardInfo();
            BalanceTextBlock.Text = info.balance;
        }
    }
}

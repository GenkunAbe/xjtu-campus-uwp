using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class TablePage : Page
    {
        public ObservableCollection<string> WeekList = new ObservableCollection<string>();
        private ObservableCollection<Course> _courses;
        private readonly TableManager _tableManager;
        public static TablePage This;
        public TablePage()
        {
            InitializeComponent();
            This = this;
            _tableManager = new TableManager();

            SetWeekList();
        }

        private void SetWeekList()
        {
            WeekList.Clear();
            WeekListCombBox.ItemsSource = WeekList;
            for (int i = 1; i <= 16; ++i)
            {
                string item = i.ToString().PadLeft(2, '0');
                WeekList.Add(item);
                if (i == UserData.NowWeek)
                    WeekListCombBox.SelectedItem = item;
            }
        }

        private async void GetNewCourses(int week = -1)
        {
            try
            {
                _courses = await _tableManager.GetCoursesList(week == -1 ? UserData.NowWeek : week, true);
            }
            catch (Exception)
            {
                Debug.WriteLine("Get Table Failed!");
            }
            SetCourses();
        }

        private void SetCourses()
        {
            TableGrid.Children.Clear();
            for (int i = 0; i < 25; ++i)
            {

                Grid grid = new Grid();
                TextBlock textBlock = new TextBlock {Style = TextBlockTileStyle};

                grid.Style = GridTileStyle;

                Course course = _courses[i];
                textBlock.Text = course.Name + "\n" + (course.Name == "" ? "" : course.Place.Substring(4));

                grid.Children.Add(textBlock);
                TableGrid.Children.Add(grid);

                var child = TableGrid.Children[i] as FrameworkElement;
                Grid.SetRow(child, i % 5);
                Grid.SetColumn(child, i / 5);
            }

        }

        private void WeekListCombBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            string seletedItem = comboBox.SelectedItem as string;
            int newWeek = int.Parse(seletedItem);
            GetNewCourses(newWeek);
        }

        public static void Refresh()
        {
            This.GetNewCourses();
        }
    }
}

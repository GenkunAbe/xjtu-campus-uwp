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
        private ObservableCollection<Course> Courses;
        private TableManager _TableManager;
        public TablePage()
        {
            this.InitializeComponent();
            
            _TableManager = new TableManager();

            WeekList.Clear();
            WeekListCombBox.ItemsSource = WeekList;
            for (int i = 1; i <= 16; ++i)
            {
                string item = (i < 10 ? "0" : "") + i;
                WeekList.Add(item);
                if (i == UserData.NowWeek)
                    WeekListCombBox.SelectedItem = item;
            }
            
        }

        private async void GetNewCourses()
        {
            Courses = await _TableManager.GetCoursesList(2, true);
            SetCourses();
        }

        private void SetCourses()
        {
            TableGrid.Children.Clear();
            for (int i = 0; i < 25; ++i)
            {

                Grid grid = new Grid();
                TextBlock textBlock = new TextBlock();

                textBlock.Style = TextBlockTileStyle;
                grid.Style = GridTileStyle;

                Course course = Courses[i];
                textBlock.Text = course.Name + "\n" + (course.Name == "" ? "" : course.Place.Substring(4));

                grid.Children.Add(textBlock);
                TableGrid.Children.Add(grid);

                var child = TableGrid.Children[i] as FrameworkElement;
                Grid.SetRow(child, i % 5);
                Grid.SetColumn(child, i / 5);
            }

        }

        private async void WeekListCombBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                try
                {
                    var seletedItem = comboBox.SelectedItem as string;
                    int nowWeek = int.Parse(seletedItem);
                    Courses = await _TableManager.GetCoursesList(nowWeek, false);
                    SetCourses();
                }
                catch (Exception)
                {
                    Debug.WriteLine("Select Week Failed!");
                }
            }
        }
    }
}

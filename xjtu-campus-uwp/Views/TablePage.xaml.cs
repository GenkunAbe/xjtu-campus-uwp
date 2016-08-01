﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using xjtu_campus_uwp.Models;


namespace xjtu_campus_uwp.Views
{
    public sealed partial class TablePage : Page
    {
        private ObservableCollection<Course> Courses;
        public TablePage()
        {
            this.InitializeComponent();
            Courses = TableManager.GetInitCourses();

            GetCourses();

            
            
        }
        private async void GetCourses()
        {

            Courses = await TableManager.GetCourse(2);

            for (int i = 0; i < 25; ++i)
            {

                Grid grid = new Grid();
                TextBlock textBlock = new TextBlock();

                textBlock.Style = TextBlockTileStyle;
                grid.Style = GridTileStyle;

                textBlock.Text = Courses[i].Name;
                
                grid.Children.Add(textBlock);
                TableGrid.Children.Add(grid);

                var child = TableGrid.Children[i] as FrameworkElement;
                Grid.SetRow(child, i % 5);
                Grid.SetColumn(child, i / 5);
            }
        }
    }
}
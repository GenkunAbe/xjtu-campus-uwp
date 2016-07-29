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
    public sealed partial class GradePage : Page
    {
        private ObservableCollection<Grade> Grades = new ObservableCollection<Grade>();
        public GradePage()
        {
            this.InitializeComponent();
            GetGrades();
        }

        private async void GetGrades()
        {
            Grades = await GradeManager.GetGrades();
            ScoreListView.ItemsSource = Grades;
        }
    }
}

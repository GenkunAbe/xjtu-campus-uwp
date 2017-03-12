using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

    public sealed partial class GradePage : Page
    {
        private readonly ObservableCollection<Grade> _grades;
        private readonly GradeManager _gradeManager = new GradeManager();
        public static GradePage This;


        public GradePage()
        {
            InitializeComponent();
            This = this;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            _grades = new ObservableCollection<Grade>();
        }

        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            if (!Frame.CanGoBack || e.Handled) return;
            e.Handled = true;
            Frame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_grades.Count != 0) return;
            GetGrades();
        }

        private async void GetGrades(bool isNew = false)
        {
            GradeRefreshProgressRing.IsActive = true;
            ObservableCollection<Grade> tmp;
            if (!isNew)
            {
                try
                {
                    tmp = await _gradeManager.GetStoredGrades();
                }
                catch (Exception)
                {
                    GetGrades(true);
                    return;
                }
            }
            else
            {
                try
                {
                    tmp = await _gradeManager.GetNewGrades();
                }
                catch (Exception)
                {
                    Debug.WriteLine("Get Grades Failed!");
                    GradeRefreshProgressRing.IsActive = false;
                    return;
                }
            }
            if (tmp.Count == 0)
            {
                Debug.WriteLine("No Grade To Show!");
                GradeRefreshProgressRing.IsActive = false;
                return;
            }
            _grades.Clear();
            foreach (Grade grade in tmp)
            {
                _grades.Add(grade);
            }
            GradeRefreshProgressRing.IsActive = false;
        }

        public static void Refresh()
        {
            This.GetGrades(true);
        }

    }
}

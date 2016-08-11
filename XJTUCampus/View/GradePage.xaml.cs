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

    public class x
    {
        public static ObservableCollection<Grade> Grades { get; set; } = new ObservableCollection<Grade>();
    }

    
    public sealed partial class GradePage : Page
    {
        public ObservableCollection<Grade> Grades
        {
            get { return x.Grades; }
            set
            {
                x.Grades.Clear();
                string nowTerm = "";
                foreach (Grade grade in value)
                {
                    if (nowTerm != grade.Term)
                    {
                        if (nowTerm != "")
                        {
                            x.Grades.Add(new Grade("", ""));
                        }
                        nowTerm = grade.Term;
                        x.Grades.Add(new Grade(nowTerm, ""));
                    }
                    x.Grades.Add(grade);
                }
            }
        }
        private static GradeManager _GradeManager = new GradeManager();

        public GradePage()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (Frame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (x.Grades.Count != 0) return;
            ObservableCollection<Grade> tmp;
            try
            {
                tmp = await _GradeManager.GetStoredGrades();
            }
            catch (Exception)
            {
                tmp = await _GradeManager.GetNewGrades();
            }
            if (tmp.Count == 0) tmp = await _GradeManager.GetNewGrades();
            Grades = tmp;
        }

        public static async void Refresh()
        {
            var tmp = await _GradeManager.GetNewGrades();
            x.Grades.Clear();
            string nowTerm = "";
            foreach (Grade grade in tmp)
            {
                if (nowTerm != grade.Term)
                {
                    if (nowTerm != "")
                    {
                        x.Grades.Add(new Grade("", ""));
                    }
                    nowTerm = grade.Term;
                    x.Grades.Add(new Grade(nowTerm, ""));
                }
                x.Grades.Add(grade);
            }
        }

    }
}

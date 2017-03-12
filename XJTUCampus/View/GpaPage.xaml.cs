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

    public sealed partial class GpaPage : Page
    {
        public ObservableCollection<Grade> Grades = new ObservableCollection<Grade>();
        public ObservableCollection<Grade> GradesBackup = new ObservableCollection<Grade>();
        public ObservableCollection<string> TermList = new ObservableCollection<string>();
        private static readonly GradeManager GradeManager = new GradeManager();
        public GpaPage()
        {
            this.InitializeComponent();
            GetGrades();
        }

        private async void GetGrades()
        {
            var tmp = await GradeManager.GetStoredGrades();
            Grades.Clear();
            GradesBackup.Clear();
            TermList.Clear();
            string term = "";
            foreach (Grade grade in tmp)
            {
                Grades.Add(grade);
                GradesBackup.Add(grade);

                if (term != grade.Term)
                {
                    term = grade.Term;
                    TermList.Add(term);
                }
            }
        }

        private void TermComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                string selectedTerm = comboBox.SelectedItem as string;
                var tmp = new ObservableCollection<Grade>();
                foreach (Grade grade in GradesBackup)
                    if (grade.Term == selectedTerm)
                        tmp.Add((grade));
                Grades.Clear();
                foreach (Grade grade in tmp)
                    Grades.Add(grade);

                GpaTitleTextBlock.FontSize = 16;
                GpaTextBlock.FontSize = 16;
                GpaTitleTextBlock.Text = "Tips: ";
                GpaTextBlock.Text = "Please select some courses.";
            }
            else
            {
                Debug.WriteLine("Selection Failed!");
            }
        }

        private void GradesListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            
            if (listView != null)
            {
                var selectedItems = listView.SelectedItems;
                try
                {
                    double scoreSum = 0.0;
                    double gpaSum = 0.0;
                    double creditSum = 0.0;
                    foreach (var item in selectedItems)
                    {
                        int score = int.Parse((item as Grade).Score);
                        double credit = double.Parse((item as Grade).Credit);
                        scoreSum += score * credit;
                        gpaSum += GradeManager.GetGpaFromScore(score) * credit;
                        creditSum += credit;
                    }

                    if (selectedItems.Count != 0)
                    {
                        GpaTitleTextBlock.FontSize = 20;
                        GpaTextBlock.FontSize = 20;
                        GpaTitleTextBlock.Text = "GPA: ";
                        GpaTextBlock.Text = string.Format("{0:00.0000} / {1:0.0000}", scoreSum / creditSum, gpaSum / creditSum);
                    }
                    else
                    {
                        GpaTitleTextBlock.FontSize = 16;
                        GpaTextBlock.FontSize = 16;
                        GpaTitleTextBlock.Text = "Tips: ";
                        GpaTextBlock.Text = "Please select some courses.";
                    }              
                }
                catch (Exception)
                {
                    GpaTitleTextBlock.FontSize = 16;
                    GpaTextBlock.FontSize = 16;

                    GpaTitleTextBlock.Text = "Error: ";
                    GpaTextBlock.Text = "Please select proper courses!";
                }
            }
            else
            {
                Debug.WriteLine("GradeListView Selection Failed!");
            }
        }
    }
}

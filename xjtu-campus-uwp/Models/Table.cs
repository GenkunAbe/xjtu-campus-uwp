using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace xjtu_campus_uwp.Models
{
    class Course
    {
        public Course(JsonArray arr)
        {
            string tmp = arr[0].GetString();
            int ind = tmp.IndexOf('(');
            int ind2 = tmp.IndexOf('（');
            Name = ind2 >= 0 ? tmp.Remove(ind2) : (ind >= 0 ? tmp.Remove(ind) : tmp);
            Teacher = arr[1].GetString();
            StartWeek = int.Parse(arr[2].GetString());
            EndWeek = int.Parse(arr[3].GetString());
            Day = arr[4].GetString();
            StartTime = int.Parse(arr[5].GetString());
            EndTime = int.Parse(arr[6].GetString());
            Place = arr[7].GetString();
            Double = int.Parse(arr[8].GetString());
        }

        public Course()
        {
            Double = -1;
        }

        public string Name { get; set; }

        public string Teacher { get; set; }

        public int StartWeek { get; set; }

        public int EndWeek { get; set; }

        public string Day { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public string Place { get; set; }

        public int Double { get; set; }
    }

    class Table
    {
        public Course[,] courses;
        public int _Now;

        public Table()
        {
            _Now = 1;
            courses = new Course[5,5];
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    courses[i, j] = new Course();
                }
            }
        }

        public Table(int Now, ObservableCollection<Course> list)
        {
            courses = new Course[5,5];
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    courses[i, j] = new Course();
                }
            }
            _Now = Now;
            foreach (var course in list)
            {
                if (course.Double != -1 && Now >= course.StartWeek && Now <= course.EndWeek)
                {
                    if ((course.Double == 1 && _Now % 2 == 0) || (course.Double == 2 && _Now % 2 == 1))
                        continue;
                    courses[GetDay(course.Day), course.StartTime/2] = course;
                }
            }
        }

        private static int GetDay(string s)
        {
            int a = 0;
            switch (s)
            {
                case "周一":
                    a = 0; break;
                case "周二":
                    a = 1; break;
                case "周三":
                    a = 2; break;
                case "周四":
                    a = 3; break;
                case "周五":
                    a = 4; break;
            }
            return a;
        }
    }

    class TableManager
    {
        public static async Task<ObservableCollection<Course>> GetCourse()
        {

            string result = await HttpHelper.GetResponse("http://202.117.14.143:12000/table?usr=genkunabe&psw=Lyx@xjtu120");

            ObservableCollection<Course> courses = new ObservableCollection<Course>();
            JsonArray divs = JsonArray.Parse(result);
            foreach (var div in divs)
            {
                JsonArray infos = JsonArray.Parse(div.ToString());
                Course course = new Course(infos);
                courses.Add(course);
            }

            System.Diagnostics.Debug.Write("GetCourse OK\n");

            return courses;
        }

        public static async Task<Table> GetTable(int week = 1)
        {
            ObservableCollection<Course> courses = await GetCourse();
            Table table = new Table(week, courses);
            System.Diagnostics.Debug.Write("GetTable OK\n");
            return table;
        } 
    }
}

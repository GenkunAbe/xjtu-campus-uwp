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
            Name = "";
            Teacher = "";
            StartWeek = -1;
            EndWeek = -1;
            Day = "";
            StartTime = -1;
            EndTime = -1;
            Place = "";
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
        public int GetDay()
        {
            int day = -1;
            switch (Day)
            {
                case "周一":
                    day = 0; break;
                case "周二":
                    day = 1; break;
                case "周三":
                    day = 2; break;
                case "周四":
                    day = 3; break;
                case "周五":
                    day = 4; break;
            }
            return day;
        }
    }


    class TableManager
    {
        public static async Task<ObservableCollection<Course>> GetCourse()
        {

            string result = await HttpHelper.GetResponse("http://192.168.0.103:12000/table?usr=genkunabe&psw=Lyx@xjtu120");

            ObservableCollection<Course> courses = new ObservableCollection<Course>();
            JsonArray divs = JsonArray.Parse(result);
            foreach (var div in divs)
            {
                JsonArray infos = JsonArray.Parse(div.ToString());
                Course course = new Course(infos);
                courses.Add(course);
            }
            return courses;
        }

        public static async Task<ObservableCollection<Course>> GetCourse(int now)
        {
            Course[,] tmpCourses = new Course[5, 5];
            ObservableCollection<Course> rawCourse = await GetCourse();

            foreach (Course course in rawCourse)
            {
                if (course.Double == -1 || now < course.StartWeek || now > course.EndWeek) continue;
                if ((course.Double == 1 && now % 2 == 0) || (course.Double == 2 && now % 2 == 1)) continue;
                tmpCourses[course.GetDay(), course.StartTime / 2] = course;
            }
            ObservableCollection<Course> courses = new ObservableCollection<Course>();
            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 5; ++j)
                    courses.Add(tmpCourses[i, j] ?? new Course());
            return courses;
        }

        public static ObservableCollection<Course> GetInitCourses()
        {
            ObservableCollection<Course> courses = new ObservableCollection<Course>();
            for (int i = 0; i < 25; ++i)
                courses.Add(new Course());
            return courses;
        } 

    }
}

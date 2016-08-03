using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

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

        private string RawCourse;
        private ObservableCollection<Course> Courses;

        public TableManager()
        {
            RawCourse = "";
            Courses = new ObservableCollection<Course>();
        }

        public async Task<ObservableCollection<Course>> GetCoursesList(int now, bool isNew)
        {
            

            Course[,] tmpCourses = new Course[5, 5];
            ObservableCollection<Course> rawCourse = new ObservableCollection<Course>();
            if (isNew)
                rawCourse = await GetNewRawCourses();
            else
                rawCourse = await GetStoredRawCourses();

            if (isNew && RawCourse == "") return GetInitCourses();
            if (!isNew && RawCourse == "") rawCourse = await GetNewRawCourses();

            foreach (Course course in rawCourse)
            {
                if (course.Double == -1 || now < course.StartWeek || now > course.EndWeek) continue;
                if ((course.Double == 1 && now % 2 == 0) || (course.Double == 2 && now % 2 == 1)) continue;
                tmpCourses[course.GetDay(), course.StartTime / 2] = course;
            }

            Courses.Clear();
            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 5; ++j)
                    Courses.Add(tmpCourses[i, j] ?? new Course());
            return Courses;
        }


        private async Task<ObservableCollection<Course>> GetNewRawCourses()
        {
            string uri = App.Host + "table?usr=" + App.NetId + "&psw=" + App.Psw;
            RawCourse = await HttpHelper.GetResponse(uri);


            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile tableFile;
            try
            {
                tableFile = await folder.GetFileAsync("table");
            }
            catch (Exception)
            {
                tableFile = await folder.CreateFileAsync("table");
            }

            try
            {
                await FileIO.WriteTextAsync(tableFile, RawCourse);
            }
            catch (Exception)
            {
                Debug.WriteLine("Write Table File Failed!");
            }


            return JsonCourseParser();
        }

        private async Task<ObservableCollection<Course>> GetStoredRawCourses()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile tableFile = await folder.GetFileAsync("table");
                RawCourse = await FileIO.ReadTextAsync(tableFile);
            }
            catch (Exception)
            {
                RawCourse = "";
                Debug.WriteLine("Open Stored Table Failed!");
            }
            return JsonCourseParser();
        }

        
        private ObservableCollection<Course> JsonCourseParser()
        {
            ObservableCollection<Course> courses = new ObservableCollection<Course>();

            if (RawCourse == "") return courses;

            JsonArray divs = JsonArray.Parse(RawCourse);            
            foreach (var div in divs)
            {
                JsonArray infos = JsonArray.Parse(div.ToString());
                Course course = new Course(infos);
                courses.Add(course);
            }
            return courses;
        }

        public static ObservableCollection<Course> GetInitCourses()
        {
            ObservableCollection<Course> courses = new ObservableCollection<Course>();
            for (int i = 0; i < 25; ++i)
                courses.Add(new Course());
            return courses;
        }

        public async Task<ObservableCollection<Course>> GetTodayCourse()
        {
            ObservableCollection<Course> courses = await GetCoursesList(App.NowWeek, false);
            string dt = DateTime.Today.DayOfWeek.ToString();
            int today = GetToday(dt);
            ObservableCollection<Course> result = new ObservableCollection<Course>();
            if (1 <= today && today <= 5)
            {
                for (int i = 0; i < 4; ++i)
                {
                    result.Add(courses[today - 1 + i]);
                }
            }
            else
            {
                for (int i = 0; i < 4; ++i)
                {
                    result.Add(new Course());
                }
            }
            return result;
        }

        private static int GetToday(string td)
        {
            switch (td)
            {
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                case "Sunday":
                    return 7;
                default:
                    return 0;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using System.Runtime.Serialization.Json;
using Windows.Data.Json;
using Windows.Storage;

namespace xjtu_campus_uwp.Models
{
    class Grade
    {
        public string Name { get; set; }
        public string Score { get; set; }

        public Grade(string name, string score)
        {
            Name = name;
            Score = score;
        }
    }



    class GradeManager
    {

        private string RawGrades;
        private ObservableCollection<Grade> Grades;

        public GradeManager()
        {
            RawGrades = "";
            Grades = new ObservableCollection<Grade>();
        }

        public async Task<ObservableCollection<Grade>> GetNewGrades()
        {
            try
            {
                string uri = App.Host + "grade?usr=" + App.NetId + "&psw=" + App.Psw;
                RawGrades = await HttpHelper.GetResponse(uri);
            }
            catch (Exception)
            {
                RawGrades = "";
                Debug.WriteLine("GetNewGrade Failed!");
            }
            JsonGradeParser();
            return Grades;
        }

        public async Task<ObservableCollection<Grade>> GetStoredGrades()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile gradeFile = await folder.GetFileAsync("grade");
                RawGrades = await FileIO.ReadTextAsync(gradeFile);
                JsonGradeParser();
                return Grades;
            }
            catch (Exception)
            {
                Debug.WriteLine("No Stored Grade!");
            }
            return new ObservableCollection<Grade>();
        }

        public async void Save()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile gradeFile;
            try
            {
                gradeFile = await folder.GetFileAsync("grade");
            }
            catch (Exception)
            {
                Debug.WriteLine("No Grade File, Create it");
                gradeFile = await folder.CreateFileAsync("grade");
            }
            try
            {
                await FileIO.WriteTextAsync(gradeFile, RawGrades);
            }
            catch (Exception)
            {
                Debug.WriteLine("Write Grade File Failed!");
            }
        }

        private void JsonGradeParser()
        {
            if (RawGrades != "")
            {
                Grades = new ObservableCollection<Grade>();
                JsonArray lines = JsonArray.Parse(RawGrades);

                foreach (IJsonValue line in lines)
                {
                    JsonArray items = JsonArray.Parse(line.ToString());
                    JsonArray scores = JsonArray.Parse(items[5].ToString());
                    Grade grade = new Grade(items[2].GetString(), scores[0].GetString());
                    // System.Diagnostics.Debug.Write(items[2].GetString() + "\n");
                    Grades.Add(grade);
                }
            }
        }
    }
}

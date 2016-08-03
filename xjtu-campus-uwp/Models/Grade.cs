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
using Windows.ApplicationModel.VoiceCommands;
using Windows.Data.Json;
using Windows.Storage;

namespace xjtu_campus_uwp.Models
{
    public class Grade
    {
        public string Term { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }

        public string Credit { get; set; }

        public string Score { get; set; }
        public string Daily { get; set; }
        public string Standard { get; set; }
        public string Interim { get; set; }
        public string Experiment { get; set; }
        public string Final { get; set; }
        public string Other { get; set; }

        public Grade(JsonArray arr)
        {
            Term = arr[0].GetString();
            Name = arr[2].GetString();
            Type = arr[3].GetString();
            Credit = arr[6].GetString();

            JsonArray scores = JsonArray.Parse(arr[5].ToString());
            Score = scores[0].GetString();
            Daily = scores[1].GetString();
            Standard = scores[2].GetString();
            Interim = scores[3].GetString();
            Experiment = scores[4].GetString();
            Final = scores[5].GetString();
            Other = scores[6].GetString();
        }

        public Grade(string name, string score)
        {
            Name = name;
            Score = score;
        }
    }



    public class GradeManager
    {

        public string RawGrades;
        public ObservableCollection<Grade> Grades;

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
                Save();
            }
            catch (Exception)
            {
                RawGrades = "";
                Debug.WriteLine("GetNewGrade Failed!");
            }
            JsonGradeParser();
            return Grades;
        }

        public async Task<List<VoiceCommandContentTile>> GetStoredGradesForCortana()
        {
            List<VoiceCommandContentTile> gradeList = new List<VoiceCommandContentTile>();
            ObservableCollection<Grade> grades = await GetStoredGrades();
            for (var i = 0; i < 6; i++)
            {
                gradeList.Add(new VoiceCommandContentTile
                {
                    AppContext = i, //用来存储该条Tile的标识  一般存储数据id
                    ContentTileType = VoiceCommandContentTileType.TitleWithText,
                    Title = grades[i].Name,
                    TextLine1 = grades[i].Score
                });
            }
            return gradeList;
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
                    JsonArray arr = JsonArray.Parse(line.ToString());
                    Grades.Add(new Grade(arr));
                }
            }
        }

        public double GetGpaFromScore(int score)
        {
            if (95 <= score && score <= 100)
                return 4.3;
            if (90 <= score && score < 95)
                return 4.0;
            if (85 <= score && score < 90)
                return 3.7;
            if (80 <= score && score < 85)
                return 3.4;
            if (75 <= score && score < 80)
                return 3.1;
            if (70 <= score && score < 75)
                return 2.8;
            if (65 <= score && score < 70)
                return 2.5;
            if (60 <= score && score < 65)
                return 2.2;
            return 1.9;
        }
    }
}

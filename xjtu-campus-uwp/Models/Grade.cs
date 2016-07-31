using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using System.Runtime.Serialization.Json;
using Windows.Data.Json;

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
        public static async Task<ObservableCollection<Grade>> GetGrades()
        {
            string result = await HttpHelper.GetResponse("http://202.117.14.143:12000/grade?usr=genkunabe&psw=Lyx@xjtu120");

            JsonArray lines = JsonArray.Parse(result);

            ObservableCollection<Grade> grades = new ObservableCollection<Grade>();

            foreach (IJsonValue line in lines)
            {
                JsonArray items = JsonArray.Parse(line.ToString());
                JsonArray scores = JsonArray.Parse(items[5].ToString());
                Grade grade = new Grade(items[2].GetString(), scores[0].GetString());
                // System.Diagnostics.Debug.Write(items[2].GetString() + "\n");
                grades.Add(grade);
            }

            // System.Diagnostics.Debug.Write("OK");
            return grades;
        }
    }
}

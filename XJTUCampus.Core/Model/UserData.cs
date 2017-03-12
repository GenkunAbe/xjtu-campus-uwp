using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XJTUCampus.Core.Model
{
    public class UserData
    {
        public static string NetId;
        public static string Psw;
        public static string PayPsw;
        public static string Host = "https://123.206.33.211:12000/";
        public static int NowWeek = 1;
        //C:\Users\genku\AppData\Local\Packages\47598GenkunAbe.XJTUCampus_g38a8qf2852k8\LocalState

        public static ObservableCollection<NewsGlance> News;
    }
}

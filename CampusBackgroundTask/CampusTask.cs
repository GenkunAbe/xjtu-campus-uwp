using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Storage;
using Windows.Web.Http;
using XJTUCampus.Model;

namespace CampusBackgroundTask
{
    public sealed class CampusTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _taskDerral;
        private VoiceCommandServiceConnection _serviceConnection;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {

            _taskDerral = taskInstance.GetDeferral();
            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            // 验证是否调用了正确的app service
            if (details == null || details.Name != "CampusService")
            {
                _taskDerral.Complete();
                return;
            }

            _serviceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(details);
            // 获取被识别的语音命令
            var cmd = await _serviceConnection.GetVoiceCommandAsync();
            switch (cmd.CommandName)
            {
                case "QueryNews":
                    await QueryNews();
                    break;
                case "QueryTable":
                    var day = cmd.Properties["Day"][0];
                    await QueryTable(day);
                    break;
                case "QueryLibrary":
                    var book = cmd.SpeechRecognitionResult.Text;
                    book = book.Substring(2);
                    await QueryBook(book);
                    break;
                case "QueryGrade":
                    await QueryGrade();
                    break;

            }
            _taskDerral.Complete();
        }

        

        private async Task QueryGrade()
        {

            // msgBack是返回给Cortana 要显示的内容
            var msgBack = new VoiceCommandUserMessage();

            // 获取成绩
            var retList = new List<VoiceCommandContentTile>();
            var gradeManager = new GradeManager();
            var gradeList = await gradeManager.GetStoredGrades();
            var N = gradeList.Count > 8 ? 8 : gradeList.Count;
            for (var i = 0; i < N; i++)
            {
                retList.Add(new VoiceCommandContentTile
                {
                    AppContext = i, //用来存储该条Tile的标识  一般存储数据id
                    ContentTileType = VoiceCommandContentTileType.TitleWithText,
                    //Image = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/300300.jpg")),
                    Title = gradeList[i].Name,
                    TextLine1 = $"学分：{gradeList[i].Credit} - 成绩：{gradeList[i].Score}"
                });
            }

            msgBack.DisplayMessage = msgBack.SpokenMessage = $"我找到了最新的{N}条成绩：";
            var response = VoiceCommandResponse.CreateResponse(msgBack, retList);
            await _serviceConnection.ReportSuccessAsync(response);

        }

        private async Task QueryTable(string day)
        {
            var msgBack = new VoiceCommandUserMessage();

            // Get Table
            int offset;
            switch (day)
            {
                case "今天":
                    offset = 0; break;
                case "明天":
                    offset = 1; break;
                case "后天":
                    offset = 2; break;
                case "昨天":
                    offset = -1; break;
                default:
                    offset = 0; break;
            }
            var retList = new List<VoiceCommandContentTile>();
            var tableManager = new TableManager();
            var tableList = await tableManager.GetTodayCourse(offset);
            VoiceCommandResponse response;
            int i = 0;
            if (tableList.Count != 6)
            {
                foreach (Course course in tableList)
                {
                    retList.Add(new VoiceCommandContentTile
                    {
                        AppContext = i,
                        ContentTileType = VoiceCommandContentTileType.TitleWithText,
                        Title = course.Name == "" ? "没课" : course.Name,
                        TextLine1 = course.Place
                    });
                }
                msgBack.DisplayMessage = msgBack.SpokenMessage = $"这是{day}的课表：";
                response = VoiceCommandResponse.CreateResponse(msgBack, retList);
            }
            else
            {
                msgBack.DisplayMessage = msgBack.SpokenMessage = $"{day}好像没课呢！";
                response = VoiceCommandResponse.CreateResponse(msgBack);
            }
            await _serviceConnection.ReportSuccessAsync(response);
        }

        private async Task QueryNews()
        {

            var newsManager = new NewsManager();
            var newsList = await newsManager.GetStoredNewsList();
            var N = newsList.Count > 10 ? 10 : newsList.Count;
            var retList = new List<VoiceCommandContentTile>();
            for (int i = 0; i < N; ++i)
            {
                retList.Add(new VoiceCommandContentTile
                {
                    AppContext = i,
                    ContentTileType = VoiceCommandContentTileType.TitleOnly,
                    Title = newsList[i].Title
                });
            }
            var msgBack = new VoiceCommandUserMessage();
            var msgRepeat = new VoiceCommandUserMessage();
            msgBack.DisplayMessage = msgBack.SpokenMessage = "这是最新的通知公告：";
            msgRepeat.DisplayMessage = msgRepeat.SpokenMessage = "选择其中的新闻以直接查看：";
            var response = VoiceCommandResponse.CreateResponseForPrompt(msgBack, msgRepeat, retList);

            var selectedRes = await _serviceConnection.RequestDisambiguationAsync(response);
            var index = (int) selectedRes.SelectedItem.AppContext;
            string link = newsList[index].Link;
            if (!link.StartsWith("http"))
                link = "http://dean.xjtu.edu.cn" + link;
            Uri uri = new Uri(link);
            await Windows.System.Launcher.LaunchUriAsync(uri);

            msgBack.DisplayMessage = msgBack.SpokenMessage = "正在打开新闻：";
            response = VoiceCommandResponse.CreateResponse(msgBack);
            await _serviceConnection.ReportSuccessAsync(response);
        }

        private async Task QueryBook(string book)
        {

            var retList = await LibraryManager.GetBookGlanceListForCortana(book);
            
            var msgGlanceBack = new VoiceCommandUserMessage();
            var msgGlanceRepeat = new VoiceCommandUserMessage();
            var msgDetailBack = new VoiceCommandUserMessage();
            var msgDetailRepeat = new VoiceCommandUserMessage();
            VoiceCommandDisambiguationResult selectedRes;
            List<VoiceCommandContentTile> retDetailList;
            VoiceCommandResponse detailResponse;

            msgGlanceBack.DisplayMessage = msgGlanceBack.SpokenMessage = $"我找到了靠前的{retList.Count}条信息：";
            msgGlanceRepeat.DisplayMessage = msgGlanceRepeat.SpokenMessage = "请选择一本图书：";
            var glanceResponse = VoiceCommandResponse.CreateResponseForPrompt(msgGlanceBack, msgGlanceRepeat, retList);

            Repeat:

            selectedRes = await _serviceConnection.RequestDisambiguationAsync(glanceResponse);

            retDetailList = await LibraryManager.GetBookDetailForCortana((string)selectedRes.SelectedItem.AppContext);
            msgDetailBack.DisplayMessage = msgDetailBack.SpokenMessage = "以下是图书详情，点击任意图书返回上级：";
            msgDetailRepeat.DisplayMessage = msgDetailRepeat.SpokenMessage = "请任意点击一本图书来返回";
            detailResponse = VoiceCommandResponse.CreateResponseForPrompt(msgDetailBack, msgDetailRepeat, retDetailList);
            await _serviceConnection.RequestDisambiguationAsync(detailResponse);

            goto Repeat;
        }
    }
}


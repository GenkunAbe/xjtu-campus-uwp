using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using xjtu_campus_uwp.Models;

namespace CampusBackgroundTask
{
    public sealed class CampusTask : IBackgroundTask
    {
        BackgroundTaskDeferral _taskDerral;
        VoiceCommandServiceConnection _serviceConnection;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("cortana");
            await FileIO.WriteTextAsync(file, "Cortana");


            _taskDerral = taskInstance.GetDeferral();
            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            _serviceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(details);


            var msgback = new VoiceCommandUserMessage();
            var msgRepeat = new VoiceCommandUserMessage();

            GradeManager gradeManager = new GradeManager();
            var gradeList = await gradeManager.GetStoredGradesForCortana();

            msgback.DisplayMessage = msgback.SpokenMessage = $"我找到了最新的{gradeList.Count}条成绩";
            msgRepeat.DisplayMessage = msgRepeat.SpokenMessage = $"我找到了最新的{gradeList.Count}条成绩";

            var response = VoiceCommandResponse.CreateResponseForPrompt(msgback, msgRepeat, gradeList);
            //var response = VoiceCommandResponse.CreateResponse(msgback, gradeList);
            await _serviceConnection.ReportSuccessAsync(response);

            _taskDerral.Complete();
        }


    }
}


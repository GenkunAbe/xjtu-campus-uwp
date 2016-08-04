using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace xjtu_campus_uwp
{

    sealed partial class App : Application
    {
        public static string NetId;
        public static string Psw;
        public static string PayPsw;
        public static string Host = "http://202.117.14.143:12000/";
        public static int NowWeek = 1;
        //C:\Users\genku\AppData\Local\Packages\fe1badb8-624b-4bb9-b17f-3efbab779028_qkgq3ram8bq14\LocalState

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                
                SplashPage splashPage = new SplashPage(e);
                Window.Current.Content = splashPage;
            }

            Window.Current.Activate();

        }

        

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            // 如果程序不是因为语音命令而激活的，就不处理
            if (args.Kind != ActivationKind.VoiceCommand) return;

            //将参数转为语音指令事件对象
            var vcargs = (VoiceCommandActivatedEventArgs)args;
            // 分析被识别的命令
            var res = vcargs.Result;
            // 获取被识别的命令的名字
            var cmdName = res.RulePath[0];
            Type navType = null;
            string propertie = null;
            //判断用户使用的是哪种语音指令
            switch (cmdName)
            {
                case "OpenMainPage":
                    navType = typeof(MainPage);
                    break;
                case "QueryFlight":
                    navType = typeof(MainPage);
                    //获取语音指令的参数
                    propertie = res.SemanticInterpretation.Properties["City"][0];
                    break;
                case "NavToPage":
                    //获取语音指令的参数
                    propertie = res.SemanticInterpretation.Properties["Destination"][0];

                    //根据 propertie 参数决定跳转到指定界面，这里就不判断了
                    navType = typeof(MainPage);
                    break;
            }
            //获取页面引用
            var root = Window.Current.Content as Frame;
            if (root == null)
            {
                root = new Frame();
                Window.Current.Content = root;
            }
            root.Navigate(navType, propertie);

            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }


    }
}

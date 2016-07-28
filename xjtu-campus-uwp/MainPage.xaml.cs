using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using xjtu_campus_uwp.Models;
using xjtu_campus_uwp.Views;

namespace xjtu_campus_uwp
{

    public sealed partial class MainPage : Page
    {
        

        public MainPage()
        {
            this.InitializeComponent();
            
            HomeFrame.Navigate(typeof (HomePage));
            NewsFrame.Navigate(typeof (NewsPage));
            LibraryFrame.Navigate(typeof (NewsPage));
            GradeFrame.Navigate(typeof (GradePage));
            CardFrame.Navigate(typeof (CardPage));

        }

        
    }
}

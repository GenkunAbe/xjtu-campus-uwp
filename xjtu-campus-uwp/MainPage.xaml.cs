using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;

            HomeFrame.Navigate(typeof (HomePage));
            NewsFrame.Navigate(typeof (NewsPage));
            TableFrame.Navigate(typeof (TablePage));
            LibraryFrame.Navigate(typeof (LibraryPage));
            GradeFrame.Navigate(typeof (GradePage));
            CardFrame.Navigate(typeof (CardPage));

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = LibraryFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = GradeFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            LibraryFrame.Navigated += OnNavigated;
            GradeFrame.Navigated += OnNavigated;

        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    HomeFrame.Navigate(typeof(HomePage));
                    break;
                case 1:
                    NewsPage.Refresh();
                    break;
                case 2:
                    TableFrame.Navigate(typeof(TablePage));
                    break;
                case 3:
                    LibraryFrame.Navigate(typeof(LibraryPage));
                    break;
                case 4:
                    GradePage.Refresh();
                    break;
                case 5:
                    CardFrame.Navigate(typeof(CardPage));
                    break;
            }
        }

        private void Gpa_Click(object sender, RoutedEventArgs e)
        {
            GradeFrame.Navigate(typeof(GpaPage));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyBook_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    Refresh.Visibility = Visibility.Visible;
                    Gpa.Visibility = Visibility.Collapsed;
                    Add.Visibility = Visibility.Collapsed;
                    MyBook.Visibility = Visibility.Collapsed;
                    Account.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    Refresh.Visibility = Visibility.Visible;
                    Gpa.Visibility = Visibility.Collapsed;
                    Add.Visibility = Visibility.Collapsed;
                    MyBook.Visibility = Visibility.Collapsed;
                    Account.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    Refresh.Visibility = Visibility.Visible;
                    Gpa.Visibility = Visibility.Collapsed;
                    Add.Visibility = Visibility.Visible;
                    MyBook.Visibility = Visibility.Collapsed;
                    Account.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    Refresh.Visibility = Visibility.Visible;
                    Gpa.Visibility = Visibility.Collapsed;
                    Add.Visibility = Visibility.Collapsed;
                    MyBook.Visibility = Visibility.Visible;
                    Account.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    Refresh.Visibility = Visibility.Visible;
                    Gpa.Visibility = Visibility.Visible;
                    Add.Visibility = Visibility.Collapsed;
                    MyBook.Visibility = Visibility.Collapsed;
                    Account.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    Refresh.Visibility = Visibility.Visible;
                    Gpa.Visibility = Visibility.Collapsed;
                    Add.Visibility = Visibility.Collapsed;
                    MyBook.Visibility = Visibility.Collapsed;
                    Account.Visibility = Visibility.Visible;
                    break;
            }
        }

        
    }
}

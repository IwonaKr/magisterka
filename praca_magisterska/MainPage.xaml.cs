using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using praca_magisterska.Resources;
using System.Diagnostics;

namespace praca_magisterska
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Debug.WriteLine("Main Page Constructor " + DateTime.Now);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Debug.WriteLine("MainPage OnNavigatedFrom " + DateTime.Now + "  " + e.ToString());
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Debug.WriteLine("MainPage OnNavigatingFrom " + DateTime.Now + "  " + e.ToString());
        }

        private void btn_gps_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GpsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btn_calls_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CallsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btn_sms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MessagesPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
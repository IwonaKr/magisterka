using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using System.Diagnostics;
using Windows.System;

namespace praca_magisterska
{
    public partial class GpsPage : PhoneApplicationPage
    {
        Geolocator geolocator;
        public GpsPage()
        {
            InitializeComponent();
            geolocator = new Geolocator();
        }
        private async void GetCoordinatesButton_Click(object sender, RoutedEventArgs e)
        {
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                loadingBar.IsEnabled = true;
                loadingBar.Visibility = Visibility.Visible;
                latitudeTextBlock.Visibility = Visibility.Collapsed;
                longitudeTextBlock.Visibility = Visibility.Collapsed;
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromSeconds(5),
                    timeout: TimeSpan.FromSeconds(10));

                latitudeTextBlock.Text = geoposition.Coordinate.Latitude.ToString();
                longitudeTextBlock.Text = geoposition.Coordinate.Longitude.ToString();
                latitudeTextBlock.Visibility = Visibility.Visible;
                longitudeTextBlock.Visibility = Visibility.Visible;
                loadingBar.IsEnabled = false;
                loadingBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBoxResult result;
                if ((uint)ex.HResult == 0x80004004)
                {
                    result = MessageBox.Show("Usługa lokalizacji jest wyłączona. Włączyć GPS?", "GPS wyłączony", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                        Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                }
                else
                {
                    result = MessageBox.Show("Nastąpił nieoczekiwany błąd podczas ustalania lokalizacji GPS", "Błąd", MessageBoxButton.OK);

                }
            }
        }
    }
}
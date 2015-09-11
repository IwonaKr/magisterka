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
using System.Threading.Tasks;

namespace praca_magisterska
{
    public partial class GpsPage : PhoneApplicationPage
    {
        Geolocator geolocator;
        public GpsPage()
        {
            InitializeComponent();
            geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            geolocator.StatusChanged += geolocator_StatusChanged;
            geolocator.PositionChanged += geolocator_PositionChanged;
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Debug.WriteLine(sender + " " + args.Position.Coordinate.Speed.ToString());
            this.setGeoposition();
        }

        private void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            Debug.WriteLine(sender + " " + args);
        }
        private void GetCoordinatesButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                
                this.setGeoposition();

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

        private async void setGeoposition()
        {

            Geoposition geoposition = await geolocator.GetGeopositionAsync(
                maximumAge: TimeSpan.FromSeconds(5),
                timeout: TimeSpan.FromSeconds(10));
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.loadingBar.IsEnabled = true;
                    this.loadingBar.Visibility = Visibility.Visible;
                    this.latitudeTextBlock.Visibility = Visibility.Collapsed;
                    this.longitudeTextBlock.Visibility = Visibility.Collapsed;


                    this.latitudeTextBlock.Text = geoposition.Coordinate.Latitude.ToString();
                    this.longitudeTextBlock.Text = geoposition.Coordinate.Longitude.ToString();
                    this.altitudeTextBlock.Text = geoposition.Coordinate.Altitude.ToString();
                    this.latitudeTextBlock.Visibility = Visibility.Visible;
                    this.longitudeTextBlock.Visibility = Visibility.Visible;
                    this.loadingBar.IsEnabled = false;
                    this.loadingBar.Visibility = Visibility.Collapsed;
                });
        }
    }
}
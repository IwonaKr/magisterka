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
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using System.Windows.Media;
using System.Windows.Shapes;

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

            geolocator.PositionChanged += geolocator_PositionChanged;
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Debug.WriteLine(sender + " " + args.Position.Coordinate.Speed.ToString());
            this.setGeoposition();
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
            GeoCoordinate geoCoordinate = new GeoCoordinate();
            geoCoordinate.Latitude = Convert.ToDouble(geoposition.Coordinate.Latitude);
            geoCoordinate.Longitude = Convert.ToDouble(geoposition.Coordinate.Longitude);
            MapOverlay myLocationOverlay;
            MapLayer mapLayer;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.loadingBar.IsEnabled = true;
                    this.loadingBar.Visibility = Visibility.Visible;
                    this.latitudeTextBlock.Visibility = Visibility.Collapsed;
                    this.longitudeTextBlock.Visibility = Visibility.Collapsed;


                    this.latitudeTextBlock.Text = geoposition.Coordinate.Latitude.ToString();
                    this.longitudeTextBlock.Text = geoposition.Coordinate.Longitude.ToString();
                    this.latitudeTextBlock.Visibility = Visibility.Visible;
                    this.longitudeTextBlock.Visibility = Visibility.Visible;
                    this.loadingBar.IsEnabled = false;
                    this.loadingBar.Visibility = Visibility.Collapsed;
                    this.mapControl.Center = geoCoordinate;
                    this.mapControl.ZoomLevel = 15;
                    Ellipse pushpin = new Ellipse();
                    pushpin.Fill = new SolidColorBrush(Colors.Black);
                    pushpin.Height = 15;
                    pushpin.Width = 15;
                    if (this.mapControl.Layers.Count > 0)
                    {
                        this.mapControl.Layers.RemoveAt(0);
                    }
                    myLocationOverlay = new MapOverlay();
                    myLocationOverlay.Content = pushpin;
                    myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                    myLocationOverlay.GeoCoordinate = geoCoordinate;

                    mapLayer = new MapLayer();
                    mapLayer.Add(myLocationOverlay);

                    this.mapControl.Layers.Add(mapLayer);
                });
        }

        public Ellipse createEllipse()
        {
            Ellipse pushpin = new Ellipse();
            pushpin.Fill = new SolidColorBrush(Colors.Black);
            pushpin.Height = 15;
            pushpin.Width = 15;
            return pushpin;
        }

        private void mapControl_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "f6102001-5399-4ff5-9e26-282085be9369";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "Zq0iVJNoEGzSAqVg6oW7kA";
        }
    }
}
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
        MapOverlay mapOverlay;
        MapLayer mapLayer;
        Ellipse pushpin;
        public GpsPage()
        {
            InitializeComponent();
            geolocator = new Geolocator();
            if (geolocator.LocationStatus.Equals(PositionStatus.Disabled))
            {
                var result = MessageBox.Show("Usługa lokalizacji jest wyłączona. Włączyć GPS?", "GPS wyłączony", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                    launchLocationSettings();
            }
            geolocator.DesiredAccuracyInMeters = 50;
            geolocator.MovementThreshold = 5;

            geolocator.PositionChanged += geolocator_PositionChanged;
            geolocator.StatusChanged += geolocator_StatusChanged;

        }

        private async static void launchLocationSettings()
        {
            bool x = await Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
        }

        void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            if (args.Status.Equals(PositionStatus.Ready))
            {
               
            }
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.latitudeTextBlock.Text = args.Position.Coordinate.Latitude.ToString();
                    this.longitudeTextBlock.Text = args.Position.Coordinate.Longitude.ToString();
                    GeoCoordinate geoCoordinate = new GeoCoordinate();
                    geoCoordinate.Latitude = Convert.ToDouble(args.Position.Coordinate.Latitude);
                    geoCoordinate.Longitude = Convert.ToDouble(args.Position.Coordinate.Longitude);
                    this.mapControl.Center = geoCoordinate;
                    this.mapControl.ZoomLevel = 15;
                    addNewPoint(geoCoordinate, out mapOverlay, out mapLayer, out pushpin);
                });
        }

        private void addNewPoint(GeoCoordinate geoCoordinate, out MapOverlay myLocationOverlay, out MapLayer mapLayer, out Ellipse pushpin)
        {
            pushpin = new Ellipse();
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
        }

        private void mapControl_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "f6102001-5399-4ff5-9e26-282085be9369";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "Zq0iVJNoEGzSAqVg6oW7kA";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using TuRuta.Client;
using TuRuta.Client.Routes;
using MapKit;
using CoreLocation;

namespace TuRuta.iOS
{
    internal class MapViewController : UIViewController
    {
        private UITableView tableView;
        private UITextField textField;
        private MKMapView mapView;
        private MapPoint[] mapPoints;
        private RoutesClient _routeClient = TuRutaClient.RoutesClientAndroid;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            tableView = new UITableView
            {
                ScrollEnabled = true,
                Hidden = true
            };

            textField = new UITextField();
            textField.ValueChanged += TextField_ValueChanged;

            mapView = new MKMapView
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleDimensions
            };

            View.AddSubviews(textField, tableView, mapView);

            var locationManger = new CLLocationManager();
            locationManger.RequestLocation();
            mapView.ShowsUserLocation = true;
            if(mapView.UserLocation != null)
            {
                var region = new MKCoordinateSpan(MilesToLatitudeDegrees(20), MilesToLongitudeDegrees(20, mapView.UserLocation.Coordinate.Latitude));
                mapView.Region = new MKCoordinateRegion(mapView.UserLocation.Coordinate, region);
            }
        }

        private async void TextField_ValueChanged(object sender, EventArgs e)
        {
            var query = sender as UITextField;
            if (query.Text.Length > 2)
            {
                tableView.Hidden = false;
                var foundRoutes = await _routeClient.Find(query.Text);
                var source = new TableSource(foundRoutes);
                source.ItemSelected += Source_ItemSelected;

                tableView.Source = source;
            }
        }

        private async void Source_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            mapView.RemoveAnnotations(mapPoints);

            var route = await _routeClient.Get(textField.Text);
            if(route?.Stops.Count != 0)
            {
                mapPoints = route.Stops.Select(stop =>
                {
                    var location = stop.Location;
                    return new MapPoint(new CLLocationCoordinate2D(location.Latitude, location.Longitude), stop.Name, stop.Id);
                }).ToArray();

                mapView.AddAnnotations(mapPoints);

                var middleStop = (route.Stops[route.Stops.Count / 2]).Location;
                var region = new MKCoordinateSpan(MilesToLatitudeDegrees(20), MilesToLongitudeDegrees(20, middleStop.Latitude));
                mapView.Region = new MKCoordinateRegion(new CLLocationCoordinate2D(middleStop.Latitude, middleStop.Longitude), region);
            }
        }

        private double MilesToLatitudeDegrees(double miles)
        {
            double earthRadius = 3960.0; // in miles
            double radiansToDegrees = 180.0 / Math.PI;
            return (miles / earthRadius) * radiansToDegrees;
        }

        private double MilesToLongitudeDegrees(double miles, double atLatitude)
        {
            double earthRadius = 3960.0; // in miles
            double degreesToRadians = Math.PI / 180.0;
            double radiansToDegrees = 180.0 / Math.PI;
            // derive the earth's radius at that point in latitude
            double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
            return (miles / radiusAtLatitude) * radiansToDegrees;
        }
    }
}
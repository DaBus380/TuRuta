using CoreLocation;
using MapKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.iOS
{
    class MapPoint : MKAnnotation
    {
        private CLLocationCoordinate2D coord;
        private string _title;
        private Guid StopId;

        public MapPoint(CLLocationCoordinate2D coordinate, string title, Guid stopId)
        {
            coord = coordinate;
            _title = title;
            StopId = stopId;
        }

        public override CLLocationCoordinate2D Coordinate 
            => coord;

        public override void SetCoordinate(CLLocationCoordinate2D value)
            => coord = value;

        public override string Title => _title;
    }
}

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using TuRuta.Client;
using TuRuta.Client.Routes;
using Android.Gms.Location;

namespace TuRuta.Droid
{
    [Activity(Label = "TuRuta", MainLauncher = true)]
    public class MainActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap Map { get; set; }
        private RoutesClient routesClient = TuRutaClient.RoutesClientAndroid;
        private AutoCompleteTextView SuggestBox { get; set; }
        private IEnumerable<string> Suggestions { get; set; }
        private FusedLocationProviderClient locationProviderClient;

        public async void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
            locationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            var location = await locationProviderClient.GetLastLocationAsync();

            if(location != null)
            {
                MoveCamera(location.Latitude, location.Longitude);
            }
        }

        private void InitMap()
        {
            var mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            if (mapFragment == null)
            {
                var options = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    .InvokeCompassEnabled(true);

                var tx = FragmentManager.BeginTransaction();
                mapFragment = MapFragment.NewInstance(options);
                tx.Add(Resource.Id.map, mapFragment, "map");
                tx.Commit();
            }

            mapFragment.GetMapAsync(this);
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.Main);

            SuggestBox = FindViewById<AutoCompleteTextView>(Resource.Id.suggestBox);
            SuggestBox.TextChanged += SuggestBox_TextChanged;
            SuggestBox.ItemClick += SuggestBox_ItemClick;

            InitMap();
        }

        private async Task SearchRoute(string hint)
        {
            Suggestions = await routesClient.Find(hint);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Suggestions.ToArray());
            SuggestBox.Adapter = adapter;
        }

        private void MoveCamera(double latitude, double longitude)
        {
            var positionBuilder = CameraPosition.InvokeBuilder();
            positionBuilder.Target(new LatLng(latitude, longitude));
            positionBuilder.Zoom(12);
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(positionBuilder.Build());
            Map.MoveCamera(cameraUpdate);
        }

        private async void SuggestBox_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var routeName = Suggestions.ElementAt(e.Position);
            var routeTask = routesClient.Get(routeName);

            Map.Clear();
            
            var route = await routeTask;

            if(route?.Stops.Count != 0)
            {
                var middlePoint = route.Stops.Count / 2;
                var middleStop = route.Stops[middlePoint];
                MoveCamera(middleStop.Location.Latitude, middleStop.Location.Longitude);

                foreach (var stop in route.Stops)
                {
                    var markerOptions = new MarkerOptions();
                    markerOptions.SetPosition(new LatLng(stop.Location.Latitude, stop.Location.Longitude));
                    markerOptions.SetTitle(stop.Name);
                    Map.AddMarker(markerOptions);
                }
            }
        }

        private async void SuggestBox_TextChanged(object sender, Android.Text.TextChangedEventArgs args)
        {
            var query = args.Text.ToString();
            if (query.Length > 2)
            {
                await SearchRoute(query);
            }
        }
    }
}


using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System.Linq;

using TuRuta.Client;
using TuRuta.Client.Routes;
using System.Collections.Generic;
using Android.Gms.Maps.Model;

namespace TuRuta.Droid
{
    [Activity(Label = "TuRuta", MainLauncher = true)]
    public class MainActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap Map { get; set; }
        private RoutesClient routesClient = TuRutaClient.RoutesClientAndroid;
        private AutoCompleteTextView SuggestBox { get; set; }
        private IEnumerable<string> Suggestions { get; set; }

        public void OnMapReady(GoogleMap googleMap) => Map = googleMap;

        private void InitMap()
        {
            var mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            if (mapFragment == null)
            {
                var options = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    .InvokeZoomControlsEnabled(true)
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

        private async void SuggestBox_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var routeName = Suggestions.ElementAt(e.Position);
            var route = await routesClient.Get(routeName);
            foreach (var stop in route.Stops)
            {
                var markerOptions = new MarkerOptions();
                markerOptions.SetPosition(new LatLng(stop.Location.Latitude, stop.Location.Longitude));
                markerOptions.SetTitle(stop.Name);
                Map.AddMarker(markerOptions);
            }
        }

        private async void SuggestBox_TextChanged(object sender, Android.Text.TextChangedEventArgs args)
        {
            var query = args.Text.ToString();
            if (query.Length > 2)
            {
                Suggestions = await routesClient.Find(query);
                var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Suggestions.ToArray());
                SuggestBox.Adapter = adapter;
            }
        }
    }
}


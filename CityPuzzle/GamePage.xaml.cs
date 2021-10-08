using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CityPuzzle
{

    public partial class GamePage : ContentPage
    {
        public GamePage(double Lng, double Lat)
        {
            InitializeComponent();
            RevealLocation(Lng, Lat);
        }

        public async void DisplayCurrLoc()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);


                if (location != null)
                {
                    Position pos = new Position(location.Latitude, location.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(pos, Distance.FromKilometers(.5));
                    map.MoveToRegion(mapSpan);
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        async void LocationTracker()
        {
            try
            {
                var location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    string x = " " + location.Latitude + " " + location.Longitude + " " + location.Altitude;
                    Locationprint(x);
                }
            }
            catch (Exception ex)
            {
                LocationError();
                // Unable to get location
            }
        }

        async void LocationError()
        {
            await DisplayAlert("Error", "Nepavyksta aptikti jusu buvimo vietos.", "OK");

        }

        async void Locationprint(string x)
        {
            await DisplayAlert("Error", x, "OK");

        }

        public void RevealLocation(double targetLat, double targetLon)
        {
            Position targetPosition = new Position(targetLat, targetLon);
            MapSpan targetSpan = MapSpan.FromCenterAndRadius(targetPosition, Distance.FromKilometers(.5));

            Pin targetPin = new Pin
            {
                Label = "Target",
                Position = targetPosition,
                Type = PinType.Generic
            };

            map.Pins.Add(targetPin);
            map.MoveToRegion(targetSpan);
        }


        void back_Click(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CityPuzzle
{

    public partial class GamePage : ContentPage
    {

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
        public GamePage()
        {
            Title = "Pins demo";

            Position position = new Position(36.9628066, -122.0194722);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);

            LocationTracker();
            Map map = new Map(mapSpan);

            Pin pin = new Pin
            {
                Label = "Santa Cruz",
                Address = "The city with a boardwalk",
                Type = PinType.Place,
                Position = position
            };
            map.Pins.Add(pin);

            Button button = new Button { Text = "Add more pins" };
            button.Clicked += (sender, e) =>
            {
                Pin boardwalkPin = new Pin
                {
                    Position = new Position(36.9641949, -122.0177232),
                    Label = "Boardwalk",
                    Address = "Santa Cruz",
                    Type = PinType.Place
                };

                boardwalkPin.MarkerClicked += async (s, args) =>
                {
                    args.HideInfoWindow = true;
                    string pinName = ((Pin)s).Label;
                    await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
                };

                Pin wharfPin = new Pin
                {
                    Position = new Position(36.9571571, -122.0173544),
                    Label = "Wharf",
                    Address = "Santa Cruz",
                    Type = PinType.Place
                };

                wharfPin.InfoWindowClicked += async (s, args) =>
                {
                    string pinName = ((Pin)s).Label;
                    await DisplayAlert("Info Window Clicked", $"The info window was clicked for {pinName}.", "Ok");
                };

                map.Pins.Add(boardwalkPin);
                map.Pins.Add(wharfPin);
            };

            Content = new StackLayout
            {
                Margin = new Thickness(10),
                Children =
                {
                    map,
                    button
                }
            };
        }
    }
}

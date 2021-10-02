using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestPage : ContentPage
    {
        private double UserLat;
        private double UserLng;
        private double QuestLat;
        private double QuestLng;
        private Puzzle[] Target;
        async Task UpdateCurrentLocation()
        {
            try
            {
               
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(60));
                var cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"UserLatitude: {location.Latitude}, UserLongitude: {location.Longitude}, Altitude: {location.Altitude}");
                    string x = " " + location.Latitude + " " + location.Longitude + " " + location.Altitude;
                    UserLat = location.Latitude;
                    UserLng = location.Longitude;
                    //CurrentLocationprint(" "+ UserLat+" "+ UserLng);
                }
                else CurrentLocationError();


            }
            catch (Exception ex)
            {
                CurrentLocationError();
                // Unable to get location
            }
        }
        async void CurrentLocationError()
        {
            await DisplayAlert("Error", "Nepavyksta aptikti jusu buvimo vietos.", "OK");

        }
        async void CurrentLocationprint(string x)
        {
            await DisplayAlert("Error", x, "OK");

        }





        public QuestPage()
        {
            InitializeComponent();
            using (SQLiteConnection conn = new SQLiteConnection(App.ObjectPath))
            {
                conn.CreateTable<Puzzle>();
                var obj = conn.Table<Puzzle>().ToArray();
                Target = obj;
            }
            if (Target.Length == 0)
            {
                Navigation.PushAsync(new AddObjectPage());
            }
            else
            {
                int num = GetQuestNumber();
                SetTargetLocation(num);

                string vieta = Target[num].ImgAdress;
                objimg.Source = vieta;

                MissionLabel.Text = "Tavo uzduotis- surasti mane!";
                QuestField.Text = Target[num].Quest;

                UpdateCurrentLocation();
            }
        }
        
        //Nera jeigu netuscias( nepabaigtas- reikia is stringo isrinti kurie nebaigti)
        public int GetQuestNumber() {
            if (App.CurrentUser.QuestsComlited == "") return 0;
            else return 0;
        }

        //Calculate distance between two map point in kilometers
       /*
        public double GetDistance()
        {
            const int R = 6371;
            var lat1 = UserLat * Math.PI / 180;
            var lat2 = QuestLat * Math.PI / 180;
            var dLat = (UserLat - QuestLat) * Math.PI / 180;
            var dLng = (UserLng - QuestLng) * Math.PI / 180;

            var Q = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +               //Haversines formula for distance between two points on a sphere
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            var havQ = 2 * Math.Atan2(Math.Sqrt(Q), Math.Sqrt(1 - Q));

            var distance = R * havQ;
            return distance;
        }
       */
        public void SetTargetLocation(int num)
        {
            QuestLat = Target[num].Latitude;
            QuestLng = Target[num].Longitude;
        }
        void check_Click(object sender, EventArgs e) {
            PrintDistance(); 
        }
        async void PrintDistance()
        {
            await UpdateCurrentLocation();
            Location start = new Location(UserLat, UserLng);
            Location end = new Location(QuestLat, QuestLng);
            string vienetai = "km";
            double dis = Location.CalculateDistance(start, end,0);
            if (dis<1) {
                vienetai = "metrai";
                dis = dis * 1000;}
             await DisplayAlert("Tau liko:"," "+ dis+" "+ vienetai, "OK") ;
        }
    }
}
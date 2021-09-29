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
        async void UpdateCurrentLocation()
        {
            try
            {

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    string x = " " + location.Latitude + " " + location.Longitude + " " + location.Altitude;
                    UserLat = location.Latitude;
                    UserLng = location.Longitude;
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

                string vieta = Target[num].ImgAdress + ".png";
                objimg.Source = vieta;

                MissionLabel.Text = "Tavo uzduotis- surasti mane!";
                QuestField.Text = Target[num].Quest;



                UpdateCurrentLocation();
            }
        }

        //Nera jeigu netuscias( nepabaigtas- reikia is stringo isrinti kurie nebaigti)
        public int GetQuestNumber()
        {
            if (App.CurrentUser.QuestsComlited == "") return 0;
            else return 0;
        }
        public double GetDistance()
        {
            return Math.Sqrt(Math.Pow((UserLat - QuestLat), 2) + Math.Pow((UserLng - QuestLng), 2));
        }

        public void SetTargetLocation(int num)
        {
            QuestLat = Target[num].Latitude;
            QuestLng = Target[num].Longitude;
        }
        void check_Click(object sender, EventArgs e)
        {
            UpdateCurrentLocation();
            printdistance();

        }
        async void printdistance()

        {
            string vienetai = "km";
            double dis = GetDistance() * 110.574;
            if (dis < 1)
            {
                vienetai = "metrai";
                dis = dis * 1000;
            }
            await DisplayAlert("Tau liko:", " " + dis + " " + vienetai, "OK");
        }

    }
}
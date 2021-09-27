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
        private double Location_X;
        private double Location_Y;
        private double PlaceLocation_X;
        private double PlaceLocation_Y;
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
                    Location_X = location.Latitude;
                    Location_Y = location.Longitude;
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
            //reikia padaryti salyga jei tuscias
            using (SQLiteConnection conn = new SQLiteConnection(App.ObjectPath))
            {
                conn.CreateTable<Puzzle>();
                var obj = conn.Table<Puzzle>().ToArray();
                Target = obj;
            }
            if (Target == null)
            {
                Navigation.PushAsync(new AddObjectPage());
            }
            int num = GetQuestNumber();
            SetTargetLocation(num);


            MissionLabel.Text = "Tavo uzduotis- surasti mane!";
            QuestField.Text = Target[num].Quest ;



            UpdateCurrentLocation();
                double dist = GetDistance();
                
         
                
            
            UpdateCurrentLocation();
        }
        
        //Nera jeigu netuscias( nepabaigtas- reikia is stringo isrinti kurie nebaigti)
        public int GetQuestNumber() {
            if (App.CurrentUser.QuestsComlited == "") return 0;
            else return 0;
        }
        public double GetDistance()
        {
            return Math.Sqrt(Math.Pow((Location_X - PlaceLocation_X),2)+ Math.Pow((Location_Y - PlaceLocation_Y), 2));
        }

        public void SetTargetLocation(int num)
        {
            PlaceLocation_X = Target[num].X;
            PlaceLocation_Y = Target[num].Y;
        }
        void check_Click(object sender, EventArgs e) {
            UpdateCurrentLocation();
            printdistance();
            
        }
        async void printdistance()

        {
            await DisplayAlert("Tau liko:", "" + GetDistance()* 110.574+" km", "OK") ;
        }

    }
}
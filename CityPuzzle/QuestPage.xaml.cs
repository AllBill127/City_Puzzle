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
                    //CurrentLocationPrint(" "+ UserLat+" "+ UserLng);
                }
                else CurrentLocationError();
            }
            catch (Exception ex)
            {
                CurrentLocationError();     // Unable to get location
            }
        }

        async void CurrentLocationError()
        {
            await DisplayAlert("Error", "Nepavyksta aptikti jusu buvimo vietos.", "OK");

        }

        async void CurrentLocationPrint(string x)
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
                ShowQuest();
            }
        }

        async private void ShowQuest()
        {
            await UpdateCurrentLocation();

            int num = GetQuestNumber(Target);
            if (num == -1)      // when no nearby quests are found. Suggest creating a new one and exit to meniu
            {
                await DisplayAlert("No destinations in " + 3 + " km radius", "Consider creating a nearby destination yourself.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                SetTargetLocation(num);

                string vieta = Target[num].ImgAdress;
                objimg.Source = vieta;

                MissionLabel.Text = "Tavo uzduotis- surasti mane!";
                QuestField.Text = Target[num].Quest;

                // TO DO: RevealImg() every smth interval of the game
            }

        }

        // Get a random index of a quest that is within given distance and is not already complete
        private int GetQuestNumber(Puzzle[] all)
        {
            List<int> inRange = new List<int>();

            for (int i = 0; i < all.Length; ++i)
            {
                Location start = new Location(UserLat, UserLng);
                Location end = new Location(all[i].Latitude, all[i].Longitude);
                double dist = Location.CalculateDistance(start, end, DistanceUnits.Kilometers);
                Console.WriteLine("\n\nIndex: " + i + " Distance: " + dist + "\n\n");
                // Currently 3km is set for distance
                // TO DO: allow user to change distance (Add Distance for objects in User class)
                // TO DO: make User class QuestComlited to List<string> type
                if (dist <= 3 /*&& !(App.CurrentUser.QuestComlited.Contains(all[i].Name))*/)
                {
                    inRange.Add(i);
                    Console.WriteLine("\n\nAdding object\n\n");
                }
            }

            if (inRange.Count != 0)
            {
                var random = new Random();
                int index = random.Next(inRange.Count);

                int num = inRange[index];
                Console.WriteLine("\n\nReturning number:" + num + "\n\n");
                return num;
            }
            else
            {
                Console.WriteLine("\n\nReturn ERROR\n\n");
                return -1;
            }
        }
        
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
            Console.WriteLine("\n\nDistanceLeft: " + dis + "\n\n");
            if (dis<1)
            {
                vienetai = "metrai";
                dis = dis * 1000;
            }
            await DisplayAlert("Tau liko:"," "+ dis+" "+ vienetai, "OK") ;
        }


        // When called hide all img masks and then reveal random masks depending on distance left
        // (when mask amount increases new random masks will be shown)
        int maskCount = 0;
        private void RevealImg()
        {
            double distStep = 3 / 9;    // TO DO: change 3 to User Distance option

            Location start = new Location(UserLat, UserLng);
            Location end = new Location(QuestLat, QuestLng);
            double distLeft = (int)Location.CalculateDistance(start, end, 0);

            int newMaskCount = 9 - (int)(distLeft / distStep);
            if (newMaskCount != maskCount)      // if maskCount increased then show new set of masks. Else skip
            {
                maskCount = newMaskCount;

                // TO DO: hide all masks
                List<int> masks = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                var random = new Random();

                for (int i = 0; i < newMaskCount; ++i)
                {
                    int index = random.Next(masks.Count);
                    masks.Remove(index);
                    // TO DO: show indexed mask
                }
            }
        }
    }
}

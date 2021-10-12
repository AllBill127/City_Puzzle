using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
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
        public static Puzzle Questinprogress;

        async Task UpdateCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(60));
                var cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
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
            
            Puzzle target = GetQuest(Target);
            if (target == null)      // when no nearby quests are found. Suggest creating a new one and exit to meniu
            {
                await DisplayAlert("No destinations in " + App.CurrentUser.maxQuestDistance + " km radius", "Consider creating a nearby destination yourself.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                QuestLat = target.Latitude;
                QuestLng = target.Longitude;
                Questinprogress = target;
                string questImg = target.ImgAdress;
                objimg.Source = questImg;

                MissionLabel.Text = "Tavo uzduotis- surasti mane!";
                QuestField.Text = target.Quest;

                await RevealImg();    // Start the quest completion loop
                App.CurrentUser.QuestComlited.Add(target.Name);            // TO DO: save user data to database after finishing quest or loging out
                await DisplayAlert("Congratulations", "You have reached the destination", "OK");
            }
        }

        // Get a random index of a quest that is within given distance and is not already complete
        private Puzzle GetQuest(Puzzle[] puzzles)
        {
            bool InRange(Puzzle puzzle)
            {
                Location start = new Location(UserLat, UserLng);
                Location end = new Location(puzzle.Latitude, puzzle.Longitude);
                double dist = Location.CalculateDistance(start, end, DistanceUnits.Kilometers);
                if (dist <= App.CurrentUser.maxQuestDistance)
                    return true;
                return false;
            }

            List<Puzzle> inRange =
            puzzles.Where(puzzle => InRange(puzzle) && !App.CurrentUser.QuestComlited.Contains(puzzle.Name)).Select(puzzle => new Puzzle
            {
                ID = puzzle.ID,
                About = puzzle.About,
                Quest = puzzle.Quest,
                Name = puzzle.Name,
                ImgAdress = puzzle.ImgAdress,
                Latitude = puzzle.Latitude,
                Longitude = puzzle.Longitude
            }).ToList();

            if (inRange.Count != 0)
            {
                var random = new Random();
                int index = random.Next(inRange.Count);

                var target = inRange[index];
                return target;
            }
            else
            {
                return null;
            }
        }
        
        void check_Click(object sender, EventArgs e) 
        {
            PrintDistance(); 
        }

        void help_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GamePage(QuestLat, QuestLng));
        }

        async void PrintDistance()
        {
            await UpdateCurrentLocation();
            Location start = new Location(UserLat, UserLng);
            Location end = new Location(QuestLat, QuestLng);

            string vienetai = "km";
            double dis = Location.CalculateDistance(start, end, 0);
            if (dis < 1)
            {
                vienetai = "metrai";
                dis = dis * 1000;
            }
            await DisplayAlert("Tau liko:", " " + dis + " " + vienetai, "OK") ;
        }


        // When called show all img masks and then reveal random masks depending on distance left
        // (when mask amount increases new random masks will be shown)
        async private Task RevealImg()
        {
            /*double distStep = distOption / 9F;      
            double distLeft = distOption;*/

            double distLeft = DistanceLeft();
            double distStep = distLeft / 9F;        

            int maskCount = 0;

            List<Image> masks = new List<Image>() { mask1, mask2, mask3, mask4, mask5, mask6, mask7, mask8, mask9 };
            var random = new Random();

            while (distLeft > 0.01)        // Quest completion loop that reveals parts of image depending on distance left
            {
                await UpdateCurrentLocation();
                distLeft = DistanceLeft();

                int newMaskCount = 9 - (int)(distLeft / distStep);      // How many masks should be hiden (9 - mask count until finish. pvz 1.9km / 0.333 = 5.7 = 5 masks left)
                
                if (newMaskCount - maskCount > 1)        // Hide more than one mask at once if it is necessary
                {
                    int count = newMaskCount - maskCount;
                    for (int i = 0; i < count; ++i)
                    {
                        maskCount += 1;
                        int index = random.Next(masks.Count);       // select random mask from the list to hide
                        masks[index].IsVisible = false;

                        masks.Remove(masks[index]);
                    }
                }

                if (newMaskCount > maskCount)      // If newMaskCount increased then hide one more mask. Else if newMaskCount decreased then "wrong direction"
                {
                    maskCount +=1;
                    int index = random.Next(masks.Count);       // select random mask from the list to hide
                    masks[index].IsVisible = false;

                    masks.Remove(masks[index]);
                    Thread.Sleep(500);
                }
                else if (newMaskCount == 9)
                {
                    Thread.Sleep(1000); 
                    await Navigation.PushAsync(new ComplitedPage(Questinprogress));
                    distLeft = 0;
                }
            }
        }

        private double DistanceLeft()
        {
            Location start = new Location(UserLat, UserLng);
            Location end = new Location(QuestLat, QuestLng);
            return Location.CalculateDistance(start, end, 0);
        }
    }
}
using CityPuzzle.Classes;
using SQLite;
using System;
using System.Linq;
using System.Collections.Generic;
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
        private List<Lazy<Puzzle>> Target;
        public static Puzzle QuestInProgress;
        public const Double HumanSpeed = 2.23;
        public const int TimeInterval = 3000;

        enum Radar
        {
            Ledas,
            Salta,
            Vidutine,
            Silta,
            Ugnis
        }

        public QuestPage()
        {
            InitializeComponent();
            Target =Sql.ReadPuzzles();
            if (Target.Count == 0)
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
                await DisplayAlert("No destinations in " + App.CurrentUser.MaxQuestDistance + " km radius", "Consider creating a nearby destination yourself.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                QuestLat = target.Latitude;
                QuestLng = target.Longitude;
                QuestInProgress = target;
                var questImg = target.ImgAdress;
                objimg.Source = questImg;

                MissionLabel.Text = "Tavo uzduotis- surasti mane!";
                QuestField.Text = target.Quest;

                await RevealImg();    // Start the quest completion loop
                App.CurrentUser.QuestsCompleted.Add(new Lazy<Puzzle>(() =>target));            // TO DO: save user data to database after finishing quest or loging out
                await DisplayAlert("Congratulations", "You have reached the destination", "OK");
            }
        }

        // Get a random quest that is within given distance and is not already completed by current user
        private Puzzle GetQuest(List<Lazy<Puzzle>> puzzles)
        {
            try
            {
                bool InRange(Lazy<Puzzle> puzzle)
                {
                    double dist = DistanceToPoint(puzzle.Value.Latitude, puzzle.Value.Longitude);
                    if (dist <= App.CurrentUser.MaxQuestDistance)
                        return true;
                    return false;
                }

                //Linq query
                //List<Puzzle> inRange = puzzles.Where(puzzle => InRange(puzzle) && !App.CurrentUser.QuestsCompleted.Contains(puzzle.Name)).ToList();
                var inRange =
                    (from puzzle in puzzles
                     where InRange(puzzle)
                     where !App.CurrentUser.QuestsCompleted.Contains(puzzle)
                     select puzzle)
                    .ToList();
                if (inRange.Count != 0)
                {
                    var random = new Random();
                    int index = random.Next(inRange.Count);
                    var target = Sql.FromLazy(inRange[index]);
                    return target;
                }
                else
                {
                    return null;
                }
            }
            catch(System.InvalidOperationException ex)
            {
                return null;
            }       
        }

        // When called show all img masks and then reveal random masks depending on distance left
        // (when mask amount increases new random masks will be hiden)
        async private Task RevealImg()
        {
            double distLeft = DistanceToPoint(QuestLat, QuestLng);
            double distStep = distLeft / 9F;

            RadarThread();

            List<Image> masks = new List<Image>() { mask1, mask2, mask3, mask4, mask5, mask6, mask7, mask8, mask9 };
            var random = new Random();

            int maskCount = 0;      // Count hiden masks

            while (distLeft > 0.01)        // Quest completion loop that reveals parts of image depending on distance left
            {
                await UpdateCurrentLocation();
                distLeft = DistanceToPoint(QuestLat, QuestLng);

                int newMaskCount = 9 - (int)(distLeft / distStep);      // How many masks should be hiden (9 - (mask count until finish. pvz 1.9km / 0.333 = 5.7 = 5 masks left) = 3 hiden)

                int count = newMaskCount - maskCount;       // If newMaskCount increased then hide more masks. Else if newMaskCount decreased then "wrong direction"
                if (count >= 1)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        maskCount += 1;
                        int index = random.Next(masks.Count);
                        masks[index].IsVisible = false;
                        masks.Remove(masks[index]);
                    }
                }
                else if (count < 0) helpbutton.IsVisible = true;
                else if (newMaskCount == 9)
                {
                    distLeft = 0;
                }
            }

            await Navigation.PushAsync(new ComplitedPage(QuestInProgress));     //When loop ends go to quest completed page
        }

        //Thread that updates current user location
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

        //Distance from user to some location. MUST CALL await UpdateCurrentLocation() before to work correctly
        private double DistanceToPoint(double pLat, double pLng)
        {
            Location start = new Location(UserLat, UserLng);
            Location end = new Location(pLat, pLng);
            return Location.CalculateDistance(start, end, 0);
        }

        void Check_Click(object sender, EventArgs e)
        {
            PrintDistance();
        }

        void Help_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GamePage(QuestLat, QuestLng));
            helpbutton.IsVisible = false;
        }

        //Displays distance to quest
        async void PrintDistance()
        {
            await UpdateCurrentLocation();
            double dist = DistanceToPoint(QuestLat, QuestLng);

            string vienetai = "km";
            if (dist < 1)
            {
                vienetai = "metrai";
                dist = dist * 1000;
            }
            
            await DisplayAlert("Tau liko:", " " + dist + " " + vienetai, "OK");
        }
        public int CountPages()
        {
            var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            return stackSize;
        }
        async void RadarThread(){
            await UpdateCurrentLocation();
            double distCheck = DistanceToPoint(QuestLat, QuestLng);
            Radar oldRadar = Radar.Vidutine;
            int startSize = CountPages();
            int nowSize = startSize;
            while (distCheck> 0.1 && startSize== nowSize)
            {   
                Thread.Sleep(TimeInterval);
                await UpdateCurrentLocation();
                double distChange = DistanceToPoint(QuestLat, QuestLng);
                double speed = ((distCheck - distChange) / TimeInterval)*1000+ HumanSpeed;

                int direction = (int)(5 * (speed) / (2 * HumanSpeed));
                if (direction > 4) direction = 4;
                else if (direction < 0) direction = 0;

                Radar state = (Radar)direction;
                if(oldRadar != state)radar.Source = state.ToString()+".gif";
                oldRadar = state;
                Console.WriteLine("Updatinu radara - i " + state.ToString() + ".gif"+" speed "+ speed+" "+ direction);
                distCheck = distChange;
                nowSize = CountPages();
            }
        }
        }
}
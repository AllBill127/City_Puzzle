using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CityPuzzle.Classes
{
    class GameLogic
    {
        private double userLat;
        private double userLng;
        private double questLat;
        private double questLng;
        private List<Lazy<Puzzle>> targets;
        private static Puzzle questInProgress;


        public delegate void RadarChangeEventDelegate(string s);    // Custom delegate for event
        public event RadarChangeEventDelegate OnRadarChange;        // Custom event
        public event EventHandler OnMaskHide;
        public event EventHandler OnNoLocationFound;
        public event EventHandler OnNoNearbyQuest;
        public event EventHandler<OnQuestStartEventArgs> OnQuestStart;
        public class OnQuestStartEventArgs : EventArgs { public string QuestImg; public string Quest; }
        public event EventHandler<OnQuestCompletedEventArgs> OnQuestCompleted;
        public class OnQuestCompletedEventArgs : EventArgs { public Puzzle QuestCompleted; }
        public event EventHandler OnNoQuestsLoaded;

        //========================================== Event trigger methods =================================================
        // OnNoQuestsLoaded trigger method
        public void StartGame()
        {
            targets = Sql.ReadPuzzles();

            if (targets.Count == 0)
            {
                OnNoQuestsLoaded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowQuest();
            }
        }

        // OnNoQuestStart and OnNoQuestCompleted trigger method
        async private void ShowQuest()
        {
            await UpdateCurrentLocation();
            Puzzle target = GetQuest(targets);
            if (target == null)      // when no nearby quests are found. Suggest creating a new one and exit to meniu
            {
                OnNoNearbyQuest?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                questLat = target.Latitude;
                questLng = target.Longitude;
                questInProgress = target;
                OnQuestStart?.Invoke(this, new OnQuestStartEventArgs { QuestImg = target.ImgAdress, Quest = target.Quest });

                Thread RadarThread = new Thread(ChangeRadar);
                RadarThread.Start();

                await RevealImg();      // Start the quest completion loop

                App.CurrentUser.QuestsCompleted.Add(new Lazy<Puzzle>(() => target));            // TO DO: save user data to database after finishing quest or loging out
                OnQuestCompleted?.Invoke(this, new OnQuestCompletedEventArgs { QuestCompleted = questInProgress });
            }
        }

        // OnMaskHide trigger method
        // Main game loop that reveals masks from the quest image depending on distance left
        private async Task RevealImg()
        {
            double distLeft = DistanceToPoint(questLat, questLng);
            double distStep = distLeft / 9F;

            var random = new Random();

            int maskCount = 0;      // Count hiden masks
            while (distLeft > 0.01)        // Quest completion loop that reveals parts of image depending on distance left
            {
                await UpdateCurrentLocation();
                distLeft = DistanceToPoint(questLat, questLng);
                int newMaskCount = 9 - (int)(distLeft / distStep);      // How many masks should be hiden (9 - (mask count until finish. pvz 1.9km / 0.333 = 5.7 = 5 masks left) = 3 hiden)
                int count = newMaskCount - maskCount;       // If newMaskCount increased then hide more masks. Else if newMaskCount decreased then "wrong direction"
                if (count >= 1)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        int index = random.Next(9 - maskCount);
                        maskCount += 1;

                        OnMaskHide?.Invoke(this, EventArgs.Empty);      // Invoke event if it has any subscribers
                    }
                }
                else if (newMaskCount == 9)
                {
                    //distLeft = 0;
                }
            }
        }

        // OnChangeRadar trigger method
        // Thread that updates radar depending on distance change and distance left 
        private enum Radar { Ledas, Salta, Vidutine, Silta, Ugnis };
        private Radar myRadar = Radar.Vidutine;

        private async void ChangeRadar()
        {
            OnRadarChange?.Invoke(Radar.Vidutine + ".gif");

            await UpdateCurrentLocation();
            double startDist = DistanceToPoint(questLat, questLng);
            double distCheck = startDist;
            double distChange;

            while (distCheck > 0.01)      // While quest location is not reached continue radar updates
            {
                await UpdateCurrentLocation();
                distChange = DistanceToPoint(questLat, questLng);

                if (distChange != distCheck)
                {
                    if (distChange < distCheck && distChange <= (startDist / 2))
                    {
                        myRadar = Radar.Ugnis;
                    }
                    else if (distChange < distCheck && distChange > (startDist / 2))
                    {
                        myRadar = Radar.Silta;
                    }
                    else if (distChange > distCheck && distChange > (startDist / 2))
                    {
                        myRadar = Radar.Ledas;
                    }
                    else if (distChange > distCheck && distChange <= (startDist / 2))
                    {
                        myRadar = Radar.Salta;
                    }

                    OnRadarChange?.Invoke(myRadar.ToString() + ".gif");
                    Thread.Sleep(5000);     // Sleep for a while so gif has time to play
                    OnRadarChange?.Invoke(Radar.Vidutine + ".gif");

                    distCheck = distChange;
                }
            }
        }

        //OnNoLocationFound trigger method
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
                    userLat = location.Latitude;
                    userLng = location.Longitude;
                }
                else OnNoLocationFound?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                OnNoLocationFound?.Invoke(this, EventArgs.Empty);
            }
        }


        //================================= Helper functions =======================================
        //Distance from user to some location. MUST CALL await UpdateCurrentLocation() before to work correctly
        private double DistanceToPoint(double pLat, double pLng)
        {
            Location start = new Location(userLat, userLng);
            Location end = new Location(pLat, pLng);
            return Location.CalculateDistance(start, end, 0);
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

                bool IsCompleted(Lazy<Puzzle> puzzle)
                {
                    try
                    {
                        Lazy<Puzzle> puz = App.CurrentUser.QuestsCompleted.SingleOrDefault(x => x.Value.ID.Equals(puzzle.Value.ID));
                        if (puz == null)
                            return false;
                        else
                            return true;
                    }
                    catch (ArgumentNullException)
                    {
                        return true;
                    }
                }
                //Linq query
                //List<Puzzle> inRange = puzzles.Where(puzzle => InRange(puzzle) && !App.CurrentUser.QuestsCompleted.Contains(puzzle.Name)).ToList();
                var inRange =
                    (from puzzle in puzzles
                     where InRange(puzzle)
                     where !IsCompleted(puzzle)
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
            catch (InvalidOperationException ex)
            {
                return null;
            }

        }

        /*
        //Displays distance to quest
        async void PrintDistance()
        {
            await UpdateCurrentLocation();
            double dist = DistanceToPoint(questLat, questLng);
            string vienetai = "km";
            if (dist < 1)
            {
                vienetai = "metrai";
                dist = dist * 1000;
            }

            await DisplayAlert("Tau liko:", " " + dist + " " + vienetai, "OK");
        }
        */
    }
}

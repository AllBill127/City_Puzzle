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
        private static object locker = new object();
        public QuestPage()
        {
            InitializeComponent();

            GameLogic gamelogic = new GameLogic();
            gamelogic.OnNoQuestsLoaded += NoQuestsLoaded;
            gamelogic.OnNoLocationFound += CurrentLocationError;
            gamelogic.OnNoNearbyQuest += NoNearbyQuest;
            gamelogic.OnQuestStart += QuestStart;
            gamelogic.OnMaskHide += HideMask;
            gamelogic.OnQuestCompleted += QuestCompleted;

            gamelogic.StartGame();
        }

        //========================================= Event subscriber methods ===============================================
        // Send user to create new puzzle if none where loaded
        private void NoQuestsLoaded(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddObjectPage());
        }

        // Alert message when current location was not found
        private async void CurrentLocationError(object sender, EventArgs e)
        {
            await DisplayAlert("Error", "Nepavyksta aptikti jusu buvimo vietos.", "OK");
        }

        // Show alert message when no quests were found around user and go back to previous window
        private async void NoNearbyQuest(object sender, EventArgs e)
        {
            await DisplayAlert("No destinations in " + App.CurrentUser.MaxQuestDistance + " km radius", "Consider creating a nearby destination yourself.", "OK");
            await Navigation.PopAsync();
        }

        // Set QuestPage variables
        private void QuestStart(object sender, GameLogic.OnQuestStartEventArgs e)
        {
            objimg.Source = e.QuestImg;
            MissionLabel.Text = "Tavo uzduotis- surasti mane!";
            QuestField.Text = e.Quest;
        }

        // Method to hide one random mask 
        private void HideMask(object sender, EventArgs e)
        {
            Thread hideMask = new Thread(HideOneMask);
            hideMask.Start();
        }

        // Show alert message when quest is completed and go to CompletedPage
        private async void QuestCompleted(object sender, GameLogic.OnQuestCompletedEventArgs e)
        {
            await DisplayAlert("Congratulations", "You have reached the destination", "OK");
            await Navigation.PushAsync(new ComplitedPage(e.QuestCompleted)); //When loop ends go to quest completed page
        }

        // Shortly reveal whole image and then return masks
        void Help_Click(object sender, EventArgs e)
        {
            Thread helpImg = new Thread(HelpImg);
            helpImg.Start();
            helpButton.IsVisible = false;
        }

        // Shuffle current masks around
        void Shuffle_Click(object sender, EventArgs e)
        {
            Thread shuffleMasks = new Thread(ShuffleMasks);
            shuffleMasks.Start();
            shuffleButton.IsVisible = false;
        }


        //=============================================== Thread methods ===================================================
        private List<int> masksIndex = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        private async void HideOneMask()
        {
            int index;
            lock (locker)
            {
                List<Image> masks = new List<Image>() { mask1, mask2, mask3, mask4, mask5, mask6, mask7, mask8, mask9 };


                var random = new Random();
                index = random.Next(masksIndex.Count);

                var mre = new ManualResetEvent(false);
                Device.BeginInvokeOnMainThread(() =>
                {
                    masks[masksIndex[index]].IsVisible = false;
                    mre.Set();      // sends mre a signal to continue
                });
                mre.WaitOne();      // stops current thread until mre receives a signal to continue

                masksIndex.Remove(masksIndex[index]);
            }
        }

        private void HelpImg()
        {
            lock (locker)
            {
                List<Image> masks = new List<Image>() { mask1, mask2, mask3, mask4, mask5, mask6, mask7, mask8, mask9 };

                foreach(var index in masksIndex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        masks[index].IsVisible = false;
                    });
                }

                Thread.Sleep(10000);

                foreach (var index in masksIndex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        masks[index].IsVisible = true;
                    });
                }
            }
        }

        private void ShuffleMasks()
        {
            lock (locker)
            {
                List<Image> masks = new List<Image>() { mask1, mask2, mask3, mask4, mask5, mask6, mask7, mask8, mask9 };
                List<int> temp = new List<int>();
                List<int> indexes = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                int maskCount = masksIndex.Count;

                var random = new Random();
                for(int i = maskCount; i > 0; --i)
                {
                    int index = random.Next(indexes.Count);
                    temp.Add(indexes[index]);
                    indexes.Remove(indexes[index]);
                }

                masksIndex = temp;

                for(int i = 0; i < 9; ++i)
                {
                    if (masksIndex.Contains(i))
                    {
                        var mre = new ManualResetEvent(false);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            masks[i].IsVisible = true;
                            mre.Set();
                        });
                        mre.WaitOne();
                    }
                    else
                    {
                        var mre = new ManualResetEvent(false);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            masks[i].IsVisible = false;
                            mre.Set();
                        });
                        mre.WaitOne();
                    }
                }
            }
        }
    }
}
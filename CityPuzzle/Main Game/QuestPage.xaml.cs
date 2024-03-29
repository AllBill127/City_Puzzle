﻿using CityPuzzle.Classes;
using SQLite;
using System;
using System.Linq;
using System.Collections.Generic;
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
        private const int showMapScoreDiff = -75;
        private const int helpScoreDiff = -50;
        private const int shuffleScoreDiff = -25;

        private GameLogic gameLogic = new GameLogic();
        private static object locker = new object();
        private const int revealTime = 10000;

        public QuestPage()
        {
            InitializeComponent();

            gameLogic.OnNoQuestsLoaded += NoQuestsLoaded;
            gameLogic.OnNoLocationFound += CurrentLocationError;
            gameLogic.OnNoNearbyQuest += NoNearbyQuest;
            gameLogic.OnQuestStart += QuestStart;
            gameLogic.OnMaskHide += HideMask;
            gameLogic.OnRadarChange += ChangeRadar;
            gameLogic.OnQuestCompleted += QuestCompleted;

            List<Puzzle> allTargets = Sql.ReadPuzzles();
            gameLogic.StartGame(allTargets);
        }

        public QuestPage(List<Puzzle> targets)
        {
            InitializeComponent();

            gameLogic.OnNoQuestsLoaded += NoQuestsLoaded;
            gameLogic.OnNoLocationFound += CurrentLocationError;
            gameLogic.OnNoNearbyQuest += NoNearbyQuest;
            gameLogic.OnQuestStart += QuestStart;
            gameLogic.OnMaskHide += HideMask;
            gameLogic.OnRadarChange += ChangeRadar;
            gameLogic.OnQuestCompleted += QuestCompleted;

            gameLogic.StartGame(targets);
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
            await DisplayAlert("Dėmesio!", "Nepavyksta aptikti jūsų buvimo vietos.", "Gerai");
        }

        // Show alert message when no quests were found around user and go back to previous window
        private async void NoNearbyQuest(object sender, EventArgs e)
        {
            await DisplayAlert("Dėmesio!", "Naujų užduočių " + App.CurrentUser.MaxQuestDistance + " km spinduliu nerasta.\nPabandykite sukurti naują užduotį.", "Gerai");

            /* Page removal from NavigationStack needs work and testing
             * var existingPages = Navigation.NavigationStack.ToList();
            existingPages.Reverse();
            foreach (var page in existingPages)
            {
                if (existingPages.Count == 3)
                    break;
                else
                    Navigation.RemovePage(page);
            }*/
            await Navigation.PopAsync();
        }

        // Set QuestPage variables
        private void QuestStart(object sender, GameLogic.OnQuestStartEventArgs e)
        {
            objImg.Source = e.QuestImg;
            missionLabel.Text = "Tavo užduotis - surasti mane!";
            questField.Text = e.Quest;
        }

        // Method to hide one random mask 
        private void HideMask(object sender, EventArgs e)
        {
            Thread hideMask = new Thread(HideOneMask);
            hideMask.Start();
        }

        // Change radar image/gif
        private void ChangeRadar(string s)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                radar.Source = s;
            });
        }

        // Show alert message when quest is completed and go to CompletedPage
        private async void QuestCompleted(object sender, GameLogic.OnQuestCompletedEventArgs e)
        {
            await DisplayAlert("Sveikiname!", "Jūs pasiekėte savo tikslą.", "Gerai");
            Console.WriteLine("iveikiau");

            await Navigation.PushAsync(new CompletedQuestPage(e.QuestCompleted, e.QuestsList, e.Score));
        }

        // Shortly reveal whole image and then return masks
        private void Help_Clicked(object sender, EventArgs e)
        {
            gameLogic.ChangeScore(helpScoreDiff);
            Thread helpImg = new Thread(HelpImg);
            helpImg.Start();
            helpButton.IsVisible = false;
        }

        // Shuffle current masks around
        private void Shuffle_Clicked(object sender, EventArgs e)
        {
            gameLogic.ChangeScore(shuffleScoreDiff);
            Thread shuffleMasks = new Thread(ShuffleMasks);
            shuffleMasks.Start();
            shuffleButton.IsVisible = false;
        }

        // Display map with pined target location
        private void ShowMap_Clicked(object sender, EventArgs e)
        {
            if (puzzleGrid.IsVisible)
            {
                puzzleGrid.IsVisible = false;
                targetMapGrid.IsVisible = true;
                SetLocation(gameLogic.GetTargetPosition());
            }
            else
            {
                puzzleGrid.IsVisible = true;
                targetMapGrid.IsVisible = false;

                gameLogic.ChangeScore(showMapScoreDiff);
                showMapButton.IsVisible = false;
            }
        }

        private void SetLocation(Position targetPosition)
        {
            MapSpan targetSpan = MapSpan.FromCenterAndRadius(targetPosition, Distance.FromKilometers(.5));
            Pin targetPin = new Pin()
            {
                Label = "Puzzle",
                Position = targetPosition,
                Type = PinType.Generic
            };

            map.Pins.Add(targetPin);
            map.MoveToRegion(targetSpan);
        }


        //=============================================== Thread methods ===================================================
        private List<int> masksIndex = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        private void HideOneMask()
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

                foreach (var index in masksIndex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        masks[index].IsVisible = false;
                    });
                }

                Thread.Sleep(revealTime);

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
                for (int i = maskCount; i > 0; --i)
                {
                    int index = random.Next(indexes.Count);
                    temp.Add(indexes[index]);
                    indexes.Remove(indexes[index]);
                }

                masksIndex = temp;

                for (int i = 0; i < 9; ++i)
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
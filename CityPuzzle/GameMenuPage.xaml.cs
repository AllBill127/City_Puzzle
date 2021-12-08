﻿using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using CityPuzzle.Classes;
using CityPuzzle.Game_Room.Join_GameRoom;


namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameMenuPage : ContentPage
    {
        public GameMenuPage()
        {
            InitializeComponent();
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestPage());
        }

        private void Leaderboard_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LeaderboardPage());
        }

        private void CreateRoom_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateRoomPage());
        }

        private void AddPuzzle_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddObjectPage());
        }

        private void JoinRoom_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SeeEnteredRooms());
        }

        private void Settings_Clicked(object sender, EventArgs e)
        {
            App.CurrentUser = null;
            Sql.SaveCurrentUser(new Classes.User("", ""));
            var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            foreach (var page in existingPages)
            {
                if (existingPages.Count == 2)
                {
                    break;
                }

                if (existingPages.Count != stackSize)
                {
                    Navigation.RemovePage(page);
                }
            }
            Navigation.PopAsync();
        }
    }

}
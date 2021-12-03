using CityPuzzle.Classes;
using CityPuzzle.Game_Room.Join_GameRoom;
using System;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameEntryPage : ContentPage
    {
        public GameEntryPage()
        {
            InitializeComponent();
        }

        void StartButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestPage());
        }

        private void Leaderboard_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LeaderboardPage());
        }

        void CreateRoom_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateGamePage());
        }

        void JoinRoom_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SeeEnteredRooms());
        }

        void AddPuzzle_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddObjectPage());
        }

        private void Settings_Clicked(object sender, EventArgs e)
        {
            App.CurrentUser = null;
            Sql.SaveCurrentUser(new User("", ""));
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
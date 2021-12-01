using CityPuzzle.Classes;
using CityPuzzle.Game_Room.Join_GameRoom;
using CityPuzzle.Side_Pages;
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
        void StartButton_click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new QuestPage());

        }
        void Create_Click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new CreateGamePage());

        }
        void Add_Click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new AddObjectPage());

        }

        void Entry_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SeeEnteredRooms());
        }

        void Button_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            string theTextFile = wc.DownloadString("https://onedrive.live.com/?authkey=%21AFs2jqf6YPPLw7k&cid=E3EB53E039BE7E4D&id=E3EB53E039BE7E4D%21540&parId=root&o=OneUp");
            Console.WriteLine(theTextFile);
        }

        private void Leaderboard_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LeaderboardPage());
        }
        private void Settings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }
    }
}
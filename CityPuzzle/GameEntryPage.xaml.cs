using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using CityPuzzle.Classes;
using CityPuzzle.Game_Room.Join_GameRoom;


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

            Navigation.PushAsync(new CreateRoomPage());

        }
        void AddPuzzle_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new AddObjectPage());

        }

        void JoinRoom_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new SeeEnteredRooms());

        }

        void Button_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            string theTextFile = wc.DownloadString("https://onedrive.live.com/?authkey=%21AFs2jqf6YPPLw7k&cid=E3EB53E039BE7E4D&id=E3EB53E039BE7E4D%21540&parId=root&o=OneUp");
            Console.WriteLine(theTextFile);
        }

        private void Settings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }
        private async void test_Click(object sender, EventArgs e)
        {
            var a = await App.WebServices.GetRoom(4);
            Console.WriteLine("Trinu"+ a.RoomPin);
        }

    }
    
}
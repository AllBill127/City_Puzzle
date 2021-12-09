using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using CityPuzzle.Classes;
using CityPuzzle.Game_Room.Join_GameRoom;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using CityPuzzle.Rest_Services.Client;

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
            //Navigation.PushAsync(new SettingsPage());
        }
        
        private void test_Clicked(object sender, EventArgs e)
        {
            Puzzle puzzle = new Puzzle() { Name = "Test_Puzzle", About = "Test_Puzzle", ImgAdress = "Test_Puzzle", Latitude = 55.00, Longitude = 100, Quest = "Test_Puzzle" };
            APICommands WebServices = new APICommands("http://10.0.2.2:5000/api/");
            puzzle.ChangeService(WebServices);
            Thread save = new Thread(() => puzzle.Save());
            save.Start();
            save.Join();
            while (puzzle.ID == 0)
                Thread.Sleep(100);
            Task<List<Puzzle>> obTask = Task.Run(() => WebServices.GetPuzzles());
            obTask.Wait();
            List<Puzzle> puzles = obTask.Result;
            Console.WriteLine("id: "+puzzle.ID);
            Console.WriteLine((puzles.Any(rt => rt.ID == puzzle.ID)));


        }
    }

}
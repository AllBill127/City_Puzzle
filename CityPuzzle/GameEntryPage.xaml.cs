using CityPuzzle.Classes;
using CityPuzzle.Game_Room.Join_GameRoom;
using System;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using CityPuzzle.Classes;
using CityPuzzle.Game_Room.Join_GameRoom;
using CityPuzzle.Rest_Services;
using CityPuzzle.Rest_Services.Model;

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
        private async void test_Click(object sender, EventArgs e)
        {
            //App.WebServices.GetParticipants();
            //App.WebServices.GetTasks();
            //App.WebServices.GetUsers();
            // App.WebServices.GetRooms();
            // App.WebServices.GetPuzzles();
            //  App.WebServices.GetRoomTasks();
            //Rest_Services.Model.Room articipant = new Rest_Services.Model.Room()
            //{
            //    id=9999,
            //    roomPin="1000",
            //    owner=9999,
            //    roomSize=8888
            //};
            //await App.WebServices.GetPuzzles();

            CompletedTask articipant = new CompletedTask()
            {
                PuzzleId=88,
                UserId=89


            };
            CompletedTask sarticipant = new CompletedTask()
            {
                PuzzleId = 88,
                UserId = 88


            };
            Task taskA = Task.Run(() => sarticipant.Save());
            taskA.Wait();
            List<CompletedTask> sa = await App.WebServices.GetTasks();
            articipant.Delete();
            //Rest_Services.Model.RoomTask articipant = new Rest_Services.Model.RoomTask()
            //{
            //    PuzzleId=10,
            //    RoomId=100
            //};
            //App.WebServices.SaveObject(articipant);

            //Console.WriteLine("SIunciu");
            //List<Rest_Services.Model.Puzzle> part = await App.WebServices.GetPuzzles();
            //Console.WriteLine("Trinu");
            //part[0].Delete();

            // App.WebServices.SaveObject(articipant);
            //HttpClientRequest.GetParticipants();
        }

    }
    
}
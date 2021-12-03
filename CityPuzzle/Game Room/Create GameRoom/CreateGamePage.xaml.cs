using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateGamePage : ContentPage
    {
        public static List<Room> AllRooms;
        public static Lazy<Room> NewRoom = new Lazy<Room>();
        public static List<int> PuzzleIds = new List<int>();
        public static Thread Data_collector_thread;
        public static int Status = -1;

        public CreateGamePage()
        {
            Data_collector_thread = new Thread(new ThreadStart(FillGameRomm));
            Data_collector_thread.Start();
            InitializeComponent();
        }
        public async static void FillGameRomm()
        {
            await CreatePin();
            NewRoom.Value.Owner = App.CurrentUser.ID;
        }
        public async static Task CreatePin()
        {
            int i = 0;
            AllRooms = Sql.ReadRooms();
            string roomPin = "";
            Random _random = new Random();
            await Task.Run(() =>
            {
                while (i == 0)
                {
                    int roomID = _random.Next(100, 10000);
                    roomPin = "kambarys" + roomID;
                    Room existing = AllRooms.SingleOrDefault(x => x.RoomPin.ToLower().Equals(roomPin.ToLower()));
                    if (existing == null)
                    {
                        i = 1;
                    }
                }
            });
            NewRoom.Value.RoomPin = roomPin;
        }

        async void AddPuzzles_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPage());
            showRoomPuzzles.IsVisible = true;
            saveRoom.IsVisible = true;
        }

        async void ShowRoomPuzzles_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectPuzzles<RoomTask>(NewRoom.Value.RoomTasks));
            NewRoom.Value.RoomTasks = SelectPuzzles<RoomTask>.getList();
        }

        async void SaveRoom_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompliteCreating());
        }
        async void LookupRooms_Clicked(object sender, EventArgs e)
        {
            List<string> usersRooms = Sql.FindParticipantRoomsIDs(App.CurrentUser.ID);
            await Navigation.PushAsync(new SelectPuzzles<string>(usersRooms));
            List<string> usersRooms2 = SelectPuzzles<string>.getList();
        }
    }
}
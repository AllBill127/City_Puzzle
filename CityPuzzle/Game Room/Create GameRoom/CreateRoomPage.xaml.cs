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
    public partial class CreateRoomPage : ContentPage
    {
        public static Lazy<Room> NewRoom = new Lazy<Room>();
        public static List<Puzzle> RoomPuzzles = new List<Puzzle>();
        public static List<int> PuzzleIds = new List<int>();

        private static Thread data_collector_thread;

        public CreateRoomPage()
        {
            InitializeComponent();

            data_collector_thread = new Thread(new ThreadStart(FillGameRomm));
            data_collector_thread.Start();
        }
        private static async void FillGameRomm()
        {
            await CreatePin();
            
        }
        private static async Task CreatePin()
        {
            // TO DO:
            // must be a better way to generate random pin and check if it does not exist already. Maybe Linq list.Any()

            int i = 0;
            var AllRooms = Sql.ReadRooms();
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

        private async void AddPuzzles_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPage(RoomPuzzles));
            PuzzleIds = RoomPuzzles.Select(x => x.ID).ToList();

            showRoomPuzzles.IsVisible = true;
            saveRoom.IsVisible = true;
        }

        private async void ShowRoomPuzzles_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectViewList<Puzzle>(RoomPuzzles));
            RoomPuzzles = SelectViewList<Puzzle>.GetList();
            PuzzleIds = RoomPuzzles.Select(x => x.ID).ToList();
        }

        private async void SaveRoom_Clicked(object sender, EventArgs e)
        {
            // TO DO:
            // show loading gif on screen
            data_collector_thread.Join();
            NewRoom.Value.Owner = App.CurrentUser.ID;
            await Navigation.PushAsync(new CompleteRoomPage());
        }
        private async void LookupRooms_Clicked(object sender, EventArgs e)
        {
            List<string> userRoomPins = Sql.ReadUserRooms().Select(room => room.RoomPin).ToList();
            await Navigation.PushAsync(new SelectViewList<string>(userRoomPins));
            // TO DO:
            // add some method to update userRooms in data base as SelectViewList allows to delete them and then return updated list with GetList()
        }
    }
}
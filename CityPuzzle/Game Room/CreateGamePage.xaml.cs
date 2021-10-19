using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateGamePage : ContentPage
    {
        public static Room NewRoom;
        public List<Puzzle> DefaultPuzzles;
        public List<string> AllUsers;
        public static int Status = -1;
        private readonly Random _random = new Random();

        public CreateGamePage()
        {
            NewRoom = new Room();
            NewRoom.ID = CreatePin();
            NewRoom.Owner = App.CurrentUser.UserName;

        InitializeComponent();
        }
        
        public string CreatePin()
        {
            int roomID = _random.Next(1, 100);
            string roomPin = "kambarys" + roomID;
            using (SQLiteConnection conn = new SQLiteConnection(App.GamePath))
            {
                conn.CreateTable<Room>();
                var AllRooms = conn.Table<Room>().ToList();
                Room existing = AllRooms.SingleOrDefault(x => x.ID.ToLower().Equals(roomPin.ToLower()));
                if (existing != null) roomPin = CreatePin();
            }
            return roomPin;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.ObjectPath))
            {
                conn.CreateTable<Puzzle>();
                var obj = conn.Table<Puzzle>().ToList();

                DefaultPuzzles = obj;
                if (Status == -1)
                {
                    AddObj_click(null, null);
                }
            }
        }

        async void AddObj_click(object sender, EventArgs e)
        {
            Status = 0;
            await Navigation.PushAsync(new AddPage());
            lookobj.IsVisible = true;
            approved.IsVisible = true;
        }

        async void Look_click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectPuzzles<Puzzle>(NewRoom.Tasks));
        }

        public static void Acction()
        {
            NewRoom.Tasks = SelectPuzzles<Puzzle>.getList();
        }

        async void Addgamer_click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CompliteCreating());
        }
    }
}
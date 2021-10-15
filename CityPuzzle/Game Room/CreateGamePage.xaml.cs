using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
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

        public CreateGamePage()
        {
            string roomId = "kambarys" + App.CurrentUser.ID;// Ateityje padaryti random su patikrinimu arba ne;
            NewRoom = new Room(roomId);
            InitializeComponent();
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
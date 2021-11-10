using CityPuzzle.Classes;
using SQLite;
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
        public static Lazy<Room> NewRoom= new Lazy<Room>();
        public List<Lazy<Puzzle>> DefaultPuzzles;
        public List<string> AllUsers;
        public static Thread Calculiator_thread=new Thread(new ThreadStart(FillGameRomm));
        public static int Status = -1;

        public CreateGamePage()
        {
            Calculiator_thread.Start();
            InitializeComponent();
            addobj.IsVisible = true;
        }
        public async static void FillGameRomm()
        {
            await CreatePin();
            NewRoom.Value.Owner = App.CurrentUser.ID;        }
        public async static Task CreatePin()
        {
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
                    Room existing = AllRooms.SingleOrDefault(x => x.ID.ToLower().Equals(roomPin.ToLower()));
                    if (existing == null) i = 1;
                }
            });
            NewRoom.Value.ID=roomPin;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DefaultPuzzles = Sql.ReadPuzzles();
            if (Status == -1)AddObj_click(null, null);

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
        
            Navigation.PushAsync(new SelectPuzzles<Lazy<Puzzle>>(NewRoom.Value.Tasks));
        }

        public static void Acction()
        {
            NewRoom.Value.Tasks = SelectPuzzles<Lazy<Puzzle>>.getList();
        }

        async void Addgamer_click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CompliteCreating());
        }
    }
}
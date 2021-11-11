using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Xml.Linq;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Threading;
using CityPuzzle.Game_Room.Join_GameRoom;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryGameRoomPage : ContentPage
    {
        public String EntryRoomID;
        public XElement grupedList;
        public Room CurrentRoom;
        public User RoomOwner;
        public delegate double Calculate(double Lat1,double Lon1, double Lat2, double Lon2);
        public EntryGameRoomPage()
        {
            InitializeComponent();
            //JoinTables();
        }
        public EntryGameRoomPage(string ID)
        {
            InitializeComponent();
            if((!SeeEnteredRooms.tReadRooms.IsCompleted || !SeeEnteredRooms.tReadUsers.IsCompleted) && !SeeEnteredRooms.tReadRooms.IsCanceled && !SeeEnteredRooms.tReadUsers.IsCanceled)
            {
                Thread.Sleep(500);
            }
            if (SeeEnteredRooms.tReadUsers.IsCanceled || SeeEnteredRooms.tReadRooms.IsCanceled) NoReadComplitedError();
            else ShowAbout(ID);
        }

        async void NoReadComplitedError()//exeotionus galima panaudoti
        {
            await DisplayAlert("Demesio", "Nepavyksta gauti duomenu. Bandykite veliau.", "OK");
            Navigation.PopAsync();
        }
        async void NoRoomFoundError()
        {
            await DisplayAlert("Demesio", "Nepavyksta aptikti kambario su Jusu GamePin.", "OK");
            Navigation.PopAsync();
        }
        async void NoOwnerFoundError()
        {
            await DisplayAlert("Demesio", "Nepavyksta aptikti duomenu susijusiu su Jusu GamePin.", "OK");
            Navigation.PopAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
       // ----------------------------Galimi exeptionai----------------------
        void ShowAbout(string EntryRoomID)
        {
            Loading.IsVisible = false;
            RoomInfo.IsVisible = true;
            Console.WriteLine("Rasau");
            foreach (Room i in SeeEnteredRooms.AllRooms)
            {
                Console.WriteLine("Turiu" + i.ID);
            }
            CurrentRoom = SeeEnteredRooms.AllRooms.SingleOrDefault(x => x.ID.Equals(EntryRoomID));
            if (CurrentRoom == null) NoRoomFoundError();
            else
            {
                PuzzleCount.Text = "" + CurrentRoom.Tasks.Count();
                RoomOwner = SeeEnteredRooms.AllUsers.SingleOrDefault(x => x.ID.Equals(CurrentRoom.Owner));
                if (RoomOwner == null) NoOwnerFoundError();
                else OwnerName.Text = RoomOwner.Name;
                Calculate distance = delegate (double Lat1, double Lon1, double Lat2, double Lon2)
                {
                    Location start = new Location(Lat1, Lon1);
                    Location end = new Location(Lat2, Lon2);
                    return Location.CalculateDistance(start, end, 0);
                };
                double totaldistance = 0;
                Lazy<Puzzle> preTask = null;
                foreach (Lazy<Puzzle> task in CurrentRoom.Tasks)
                {
                    if (preTask == null) preTask = task;
                    else
                    {
                        totaldistance += distance(preTask.Value.Latitude, preTask.Value.Longitude, task.Value.Latitude, task.Value.Longitude);
                        preTask = task;
                    }
                }
                RoadDistance.Text = totaldistance + "km";
            }
        }
        public async void FillXamlData()
        {
            PuzzleCount.Text = "" + CurrentRoom.Tasks.Count();


        }
    }
}
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
        private string entryRoomID;
        private Room currentRoom;
        private User roomOwner;
        private delegate double calculate(double Lat1, double Lon1, double Lat2, double Lon2);

        public EntryGameRoomPage()
        {
            InitializeComponent();
        }
        public EntryGameRoomPage(string ID)
        {
            InitializeComponent();
            if (SeeEnteredRooms.AllRooms == null || SeeEnteredRooms.AllUsers == null) NoReadComplitedError();
            else ShowAbout(ID);
        }
        public EntryGameRoomPage(Room room)
        {
            InitializeComponent();
            if (SeeEnteredRooms.AllRooms == null || SeeEnteredRooms.AllUsers == null) NoReadComplitedError();
            else ShowAbout(room);
        }

        async void NoReadComplitedError()//exeotionus galima panaudoti
        {
            await DisplayAlert("Demesio", "Nepavyksta gauti duomenu. Bandykite veliau.", "OK");
            await Navigation.PopAsync();
        }
        async void NoRoomFoundError()
        {
            await DisplayAlert("Demesio", "Nepavyksta aptikti kambario su Jusu GamePin.", "OK");
            await Navigation.PopAsync();
        }
        async void NoOwnerFoundError()
        {
            await DisplayAlert("Demesio", "Nepavyksta aptikti duomenu susijusiu su Jusu GamePin.", "OK");
            await Navigation.PopAsync();
        }
        async void CompitedJoin()
        {
            await DisplayAlert("Sveikiname", "Jus dalyvaujate GameRoome.", "OK");
            await Navigation.PopAsync();
        }
        /*protected override void OnAppearing()
        {
            base.OnAppearing();

        }*/

        void ShowAbout(string EntryRoomID)
        {
            Loading.IsVisible = false;
            RoomInfo.IsVisible = true;
            currentRoom = SeeEnteredRooms.AllRooms.SingleOrDefault(x => x.RoomPin.Equals(EntryRoomID));
            if (currentRoom == null) NoRoomFoundError();/// exeptionas
            else
            {
                PuzzleCount.Text = "" + currentRoom.RoomTasks.Count();
                roomOwner = SeeEnteredRooms.AllUsers.SingleOrDefault(x => x.ID.Equals(currentRoom.Owner));
                if (roomOwner == null) NoOwnerFoundError();/// exeptionas
                else OwnerName.Text = roomOwner.Name;
                RoomPinas.Text = EntryRoomID;
                //calculate distance = delegate (double Lat1, double Lon1, double Lat2, double Lon2)
                //{
                //    Location start = new Location(Lat1, Lon1);
                //    Location end = new Location(Lat2, Lon2);
                //    return Location.CalculateDistance(start, end, 0);
                //};
                //double totaldistance = 0;
                //Lazy<Puzzle> preTask = null;
                //foreach (Lazy<Puzzle> task in CurrentRoom.Tasks)
                //{
                //    if (preTask == null) preTask = task;
                //    else
                //    {
                //        totaldistance += distance(preTask.Value.Latitude, preTask.Value.Longitude, task.Value.Latitude, task.Value.Longitude);
                //        preTask = task;
                //    }
                //}
                //RoadDistance.Text = totaldistance + "km";
            }
        }
        void ShowAbout(Room room)
        {
            Loading.IsVisible = false;
            RoomInfo.IsVisible = true;
            currentRoom = room;
            PuzzleCount.Text = "" + currentRoom.RoomTasks.Count();
            roomOwner = SeeEnteredRooms.AllUsers.SingleOrDefault(x => x.ID.Equals(currentRoom.Owner));
            if (roomOwner == null) NoOwnerFoundError();
            OwnerName.Text = roomOwner.Name;
            RoomPinas.Text = entryRoomID;
            //calculate distance = delegate (double Lat1, double Lon1, double Lat2, double Lon2)
            //     {
            //         Location start = new Location(Lat1, Lon1);
            //         Location end = new Location(Lat2, Lon2);
            //         return Location.CalculateDistance(start, end, 0);
            //     };
            //double totaldistance = 0;
            //Lazy<Puzzle> preTask = null;
            //foreach (Puzzle task in CurrentRoom.)
            //{
            //    if (preTask == null) preTask = task;
            //    else
            //    {
            //        totaldistance += distance(preTask.Value.Latitude, preTask.Value.Longitude, task.Value.Latitude, task.Value.Longitude);
            //        preTask = task;
            //    }
            //}
            //RoadDistance.Text = totaldistance + "km";
        }
    
    void Start_Click(object sender, EventArgs e)
    {
        Sql.SaveParticipants(currentRoom.RoomPin, App.CurrentUser.ID);
        CompitedJoin();
        Navigation.PopAsync();
    }
    void Skip_Click(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}
}
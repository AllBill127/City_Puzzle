using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Xml.Linq;
using Xamarin.Essentials;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryGameRoomPage : ContentPage
    {
        public String EntryRoomID;
        public List<Room> AllRooms = new List<Room>();
        public List<User> AllUsers= new List<User>();
        public XElement grupedList;
        public Room CurrentRoom;
        public User RoomOwner;
        public delegate double Calculate(double Lat1,double Lon1, double Lat2, double Lon2);
        public EntryGameRoomPage()
        {
            InitializeComponent();
            //JoinTables();
        }

        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AllRooms = Sql.ReadRooms();
            AllUsers = Sql.ReadUsers();
        }
        void ReadPin(object sender, EventArgs e)
        {
            EntryRoomID = CheckPin.Text;
            CurrentRoom = AllRooms.SingleOrDefault(x => x.ID.Equals(EntryRoomID));
            if (CurrentRoom != null) ShowAbout();
        }
        void ShowAbout()
        {
            PuzzleCount.Text = "" + CurrentRoom.Tasks.Count();
            RoomOwner= AllUsers.SingleOrDefault(x => x.ID.Equals(CurrentRoom.Owner));
            OwnerName.Text = RoomOwner.Name;
            Calculate distance = delegate (double Lat1, double Lon1, double Lat2, double Lon2) {
                Location start = new Location(Lat1, Lon1);
                Location end = new Location(Lat2, Lon2);
                return Location.CalculateDistance(start, end, 0);
            };
            double totaldistance=0;
            Lazy<Puzzle> preTask = null;
            foreach (Lazy<Puzzle> task in CurrentRoom.Tasks)
            {
                if (preTask == null) preTask = task;
                else
                {
                    totaldistance += distance(preTask.Value.Latitude, preTask.Value.Longitude,task.Value.Latitude, task.Value.Longitude);
                    preTask = task;
                }   
            }
            RoadDistance.Text = totaldistance + "km";
        }
    }
}
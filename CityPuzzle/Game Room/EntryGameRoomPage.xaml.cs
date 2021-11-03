using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Xml.Linq;

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
        public EntryGameRoomPage()
        {
            InitializeComponent();
            //JoinTables();
        }
        /*
        public void JoinTables()
        {
            /*---------------------
        grupedList = new XElement("GameOwners",
        from User in AllUsers
        join Room in AllRooms on User.UserName equals Room.Owner into ownerName
        select new XElement("Owner",
            new XAttribute("OwnerID", User.ID),
            new XAttribute("OwnerName", User.Name),
            new XAttribute("OwnerLastName", User.LastName),
            new XAttribute("OwnerUserName", User.UserName),
            from Room in ownerName
            select new XElement("Room",
            new XAttribute("Size", Room.RoomSize),
            new XAttribute("Tasks", Room.Tasks),
            new XAttribute("Participants", Room.Participants)
            )));

            Console.WriteLine(grupedList);
        }

        async void getRoomId()
        {
            string message = await DisplayPromptAsync("Dalyvavimas dalyviu žaidime", "Ivesk Room ID?");
            
               CurrentRoom = AllRooms.SingleOrDefault(x => x.ID.ToLower().Equals(message.ToLower()));
   
        }
        public void setValues()
        {
            var groupJoin = AllUsers.GroupJoin(AllRooms,
                       User => User.Name,
                       Room => Room.Owner,
                       (User, RoomCollection) =>
                           new
                           {
                               OwnerName = User.Name,
                               Quests = RoomCollection
                           });
            var CurentRoomVar = false;
            OwnerName.Text = "zero";
            foreach (var item in groupJoin)
            {
                foreach (var room in item.Quests)
                    if (room.ID == EntryRoomID)
                    {
                        OwnerName.Text = room.Owner;
                        PuzzleCount.Text = " "+room.Tasks.Count();
                    }
            }
            if (OwnerName.Text == "zero")  DisplayAlert("ISpejimas", "Nerastas PIN", "OK");

        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.GamePath))
            {
                //conn.DeleteAll<Room>();
                conn.CreateTable<Room>();
                AllRooms = conn.Table<Room>().ToList();
            }
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<User>();
                AllUsers = conn.Table<User>().ToList();
            }
           
           // var CurentRoomVar = groupJoin.SingleOrDefault(x => x.Quests[y].ID.ToLower().Equals(message.ToLower()));
        }*/
        void ReadPin(object sender, EventArgs e)
        {
            EntryRoomID = CheckPin.Text;
           // setValues();
        }
    }
}
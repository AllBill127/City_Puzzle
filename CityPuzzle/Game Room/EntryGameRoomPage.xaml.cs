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
        public Room CurrentRoom;
        public EntryGameRoomPage()
        {
            InitializeComponent();
            getRoomId();
        }

        public void ShowInfo()
        {
        XElement grupedList = new XElement("GameOwners",
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
                ShowInfo();



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
        }

    }
}
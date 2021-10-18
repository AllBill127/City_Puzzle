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
        from user in AllUsers
        join Room in AllRooms on user.UserName equals Room.Owner into ownerName
        select new XElement("Owner",
            new XAttribute("FirstName", person.FirstName),
            new XAttribute("LastName", person.LastName),
            from subpet in ownerName
            select new XElement("Pet", subpet.Name)));
        }

        async void getRoomId()
        {
            string message = await DisplayPromptAsync("Dalyvavimas dalyviu žaidime", "Ivesk Room ID?");
            try
            {
                CurrentRoom = AllRooms.SingleOrDefault(x => x.ID.ToLower().Equals(message.ToLower()));
                ShowInfo();

            }
            catch
            {
                await DisplayAlert("Ispejimas: ","Neatrastas atitikmuo", "OK");
            }

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.GamePath))
            {
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
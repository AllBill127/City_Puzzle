using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryGameRoomPage : ContentPage
    {
        public String EntryRoomID;
        public List<Room> AllRooms = new List<Room>();
        public Room CurrentRoom;
        public EntryGameRoomPage()
        {
            InitializeComponent();
            getRoomId();


        }
        async void getRoomId()
        {
            string message = await DisplayPromptAsync("Dalyvavimas dalyviu žaidime", "Ivesk Room ID?");
            try
            {
                CurrentRoom = AllRooms.SingleOrDefault(x => x.ID.ToLower().Equals(message.ToLower()));

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
            }}

    }
}
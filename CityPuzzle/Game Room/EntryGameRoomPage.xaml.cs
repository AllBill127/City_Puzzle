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

        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AllRooms = Sql.ReadRooms();
            AllUsers = Sql.ReadUsers();
        }
        void ReadPin(object sender, EventArgs e)
        {
            EntryRoomID = CheckPin.Text;
        }
    }
}
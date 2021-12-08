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
        private Room currentRoom;
        private User roomOwner;

        public EntryGameRoomPage()
        {
            InitializeComponent();
        }

        public EntryGameRoomPage(string ID)
        {
            InitializeComponent();
            try
            {
                ShowAbout(ID);
            }
            catch (RoomNotFoundException ex)
            {
                DisplayErrorMessage(ex.Message);
            }
            catch (OwnerNotFoundException ex)
            {
                DisplayErrorMessage(ex.Message);
            }
        }

        void ShowAbout(string roomID)
        {
            roomInfo.IsVisible = true;
            currentRoom = SeeEnteredRooms.AllRooms.SingleOrDefault(x => x.RoomPin.Equals(roomID));

            if (currentRoom == null)
                throw new RoomNotFoundException();
            else
            {
                puzzleCount.Text = "" + currentRoom.RoomTasks.Count();
                roomOwner = SeeEnteredRooms.AllUsers.SingleOrDefault(x => x.ID.Equals(currentRoom.Owner));

                if (roomOwner == null)
                    throw new OwnerNotFoundException();
                else
                    ownerName.Text = roomOwner.Name;

                roomPin.Text = roomID;
            }
        }

        private async void DisplayErrorMessage(string errMsg)
        {
            await DisplayAlert("Dėmesio!", errMsg, "Gerai");
            await Navigation.PopAsync();
        }

        private async void Start_Clicked(object sender, EventArgs e)
        {
            var participant = new Participant() { UserId = App.CurrentUser.ID, RoomId = currentRoom.ID };
            participant.Save();

            await DisplayAlert("Sveikiname", "Jus dalyvaujate GameRoome.", "OK");
            await Navigation.PopAsync();
        }

        private void LeaveRoom_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
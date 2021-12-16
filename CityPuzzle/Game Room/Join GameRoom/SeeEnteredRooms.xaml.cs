using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle.Game_Room.Join_GameRoom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeeEnteredRooms : ContentPage
    {
        public static List<Room> AllRooms = new List<Room>();
        public static List<User> AllUsers = new List<User>();

        private static List<Room> userRooms = new List<Room>();
        private delegate void Change();

        public SeeEnteredRooms()
        {
            InitializeComponent();
            ShowEnteredRoom();
        }

        private async void ShowEnteredRoom()
        {
            await Task.Run(() => Refreash());
        }

        // Change page view elements by executing lines in Change del()
        private void ChangeView(Change del)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                del();
            });
        }

        private void Refreash()
        {
            // Display loading gif while all rooms and users are read
            Thread tReadRooms = new Thread(() => { AllRooms = Sql.ReadRooms(); });
            Thread tReadUsers = new Thread(() => { AllUsers = Sql.ReadUsers(); });
            tReadRooms.Start();
            tReadUsers.Start();
            tReadRooms.Join();
            tReadUsers.Join();

            // Display loading gif in user rooms window while they are read
            ChangeView(delegate ()
            {
                loadingView.IsVisible = false;
                roomMenuView.IsVisible = true;
            });

            userRooms = Sql.ReadUserRooms();

            // Display 'No rooms found' in user rooms window
            if (userRooms == null)
            {
                ChangeView(delegate ()
                {
                    noRoomsView.IsVisible = true;
                    smallLoadingView.IsVisible = false;
                });
            }
            // Display list of user rooms in user rooms window
            else
            {
                ChangeView(delegate ()
                {
                    roomListView.ItemsSource = userRooms;
                    roomListView.IsVisible = true;
                    smallLoadingView.IsVisible = false;
                });
            }
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                AskIfContinueRoom(userRooms[e.ItemIndex]);
            }
            roomListView.SelectedItem = null;
        }

        private async void RoomSignIn_Clicked(object sender, EventArgs e)
        {
            try
            {
                string roomPin = gamePin.Text;
                Room currentRoom = userRooms.SingleOrDefault(x => x.RoomPin.Equals(roomPin));

                if (currentRoom != null)
                    throw new MultiRegistrationException(currentRoom);

                currentRoom = AllRooms.SingleOrDefault(x => x.RoomPin.Equals(roomPin));

                if (currentRoom == null)
                    throw new RoomNotExistException();

                CheckAvailability(currentRoom);

                await Navigation.PushAsync(new EntryGameRoomPage(roomPin));
            }
            catch (RoomFullException exception)
            {
                await DisplayAlert("Dėmesio!", exception.Message, "Gerai");
            }
            catch (RoomNotExistException exception)
            {
                await DisplayAlert("Dėmesio!", exception.Message, "Gerai");
            }
            catch (MultiRegistrationException exception)
            {
                RoomExistsError(exception.CurrentRoom, exception.Message);
            }
        }

        private async void AskIfContinueRoom(Room selectedRoom)
        {
            bool answer = await DisplayAlert("Dėmesio!", "Ar norite tęsti žaidimą - " + selectedRoom.RoomPin, "Taip", "Ne");
            if (answer)
            {
                var allPuzzles = Sql.ReadPuzzles();
                var roomPuzzles = allPuzzles.Where(x => x.ID.Equals(selectedRoom)).ToList();

                await Navigation.PushAsync(new QuestPage(roomPuzzles));
            }
        }

        private async void RoomExistsError(Room selectedRoom, string msg)
        {
            bool answer = await DisplayAlert("Dėmesio!", msg, "Taip", "Ne");
            if (answer)
                AskIfContinueRoom(selectedRoom);
        }

        private void CheckAvailability(Room selectedRoom)
        {
            if (selectedRoom.Participants.Count >= selectedRoom.RoomSize)
                throw new RoomFullException();
        }
    }
}
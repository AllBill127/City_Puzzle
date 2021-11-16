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
        private static List<Room> EnteredRooms = new List<Room>();
        private delegate void Change();

        public SeeEnteredRooms()
        {
            InitializeComponent();
            tRefreash();
        }
        private async void tRefreash()
        {
            await Task.Run(() =>
            {
                Refreash();
            });
        }
        private void ChangeView(Change del)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                del();
            });
        }
        private void Refreash()
        {
            Thread tReadRooms = new Thread(() => { AllRooms = Sql.ReadRooms(); });
            Thread tReadUsers = new Thread(() => { AllUsers = Sql.ReadUsers(); });
            tReadRooms.Start();
            tReadUsers.Start();
            while (tReadRooms.IsAlive || tReadUsers.IsAlive)
                Thread.Sleep(300);
            ChangeView(delegate () { LoadingGrid.IsVisible = false; MainStack.IsVisible = true; });
            Task<List<Room>> taskFindRooms = new Task<List<Room>>(() => { return GetData(); });
            taskFindRooms.Start();
            Task.WaitAll();

            if (taskFindRooms.Result == null)
                ChangeView(delegate () { NoRooms.IsVisible = true; });
            else
            {
                EnteredRooms = taskFindRooms.Result;
                ChangeView(delegate ()
                {
                    MyListView.ItemsSource = taskFindRooms.Result;
                    MyListView.IsVisible = true;
                });
            }
            ChangeView(delegate () { LoadingSmallGrid.IsVisible = false; });
        }
        private static List<Room> GetData()
        {
            List<Room> foundRooms = new List<Room>();
            Task<List<string>> tFind = new Task<List<string>>(
              () =>
              {
                  return Sql.FindParticipantRoomsIDs(App.CurrentUser.ID);
              });
            tFind.Start();
            Task.WaitAll();
            foreach (string pin in tFind.Result)
            {
                foundRooms.Add(AllRooms.SingleOrDefault(x => x.ID == pin));
            }
            return foundRooms;
        }
        private async void Sign_Click(object sender, EventArgs e)
        {
            try
            {
                string gamePin = GamePin.Text;
                Room current = EnteredRooms.SingleOrDefault(x => x.ID.Equals(gamePin));
                if (current != null)
                    throw new MultiRegistrationException(current);
                current = AllRooms.SingleOrDefault(x => x.ID.Equals(gamePin));
                if (current == null)
                    throw new RoomNotExistException();
                CheckAvailability(current);
                await Navigation.PushAsync(new EntryGameRoomPage(gamePin));
            }
            catch (RoomFullException exception)
            {
                await DisplayAlert("Dėmesio!", exception.Message, "Gerai");
            }
            catch (RoomNotExistException exception)
            {
                await DisplayAlert ("Dėmesio!", exception.Message, "Gerai");
            }
            catch (MultiRegistrationException exception)
            {
                RoomExistsError(exception.CurrentRoom, exception.Message);
            }
        }
        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                AskIfContinueRoom(EnteredRooms[e.ItemIndex]);
            }
            MyListView.SelectedItem = null;
            Console.WriteLine(" " + e.Item);
        }
        private async void AskIfContinueRoom(Room selectedRoom)
        {
            bool answer = await DisplayAlert("Dėmesio!", "Ar norite tęsti žaidimą - " + selectedRoom.ID, "Taip", "Ne");
            if (answer)
                await Navigation.PushAsync(new QuestPage(selectedRoom.Tasks));
        }
        private async void RoomExistsError(Room selectedRoom, string msg)
        {
            bool answer = await DisplayAlert("Dėmesio!", msg, "Taip", "Ne");
            if (answer)
                AskIfContinueRoom(selectedRoom);
        }
        private void CheckAvailability(Room selectedRoom)
        {
            if (selectedRoom.ParticipantIDs.Count >= selectedRoom.RoomSize)
                throw new RoomFullException();
        }
    }
}
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
        public static List<Room> EnteredRooms = new List<Room>();
        public delegate void Change();
        public SeeEnteredRooms()
        {
            InitializeComponent();
            tRefreash();
        }
        public async void tRefreash()
        {
            await Task.Run(() =>
            {
                Refreash();
            });
        }
        public void ChangeView(Change del)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                del();
            });
        }
        public void Refreash()
        {
            Thread tReadRooms = new Thread(() => { AllRooms = Sql.ReadRooms(); });
            Thread tReadUsers = new Thread(() => { AllUsers = Sql.ReadUsers(); });
            tReadRooms.Start();
            tReadUsers.Start();
            while (tReadRooms.IsAlive || tReadUsers.IsAlive) Thread.Sleep(300);
            ChangeView(delegate () { LoadingGrid.IsVisible = false; MainStack.IsVisible = true; });//dar vienas anonimas
            Task<List<Room>> treadFindRooms = new Task<List<Room>>(() => { return GetData(); });
            treadFindRooms.Start();
            Task.WaitAll();
            if (treadFindRooms.Result == null) ChangeView(delegate () { NoRooms.IsVisible = true; });
            else
            {
                EnteredRooms = treadFindRooms.Result;
                ChangeView(delegate ()
                {
                    MyListView.ItemsSource = treadFindRooms.Result;
                    MyListView.IsVisible = true;
                });
            }
            ChangeView(delegate () { LoadingSmallGrid.IsVisible = false; });
        }
        public static List<Room> GetData()
        {
            List<Room> foundRooms  = new List<Room>();
            Task<List<string>> tFind = new Task<List<string>>(
              () =>
              {
                  return Sql.FindParticipantRoomsIDs(App.CurrentUser.ID);
              });
            tFind.Start();
            Task.WaitAll();
            foreach (string pin in tFind.Result)
            {
                foundRooms .Add(AllRooms.SingleOrDefault(x => x.ID == pin));
            }
            return foundRooms ;
        }
        void Sign_Click(object sender, EventArgs e)
        {
            try
            {
                string gamePin = GamePin.Text;
                Room current = EnteredRooms.SingleOrDefault(x => x.ID.Equals(gamePin));
                if (current != null) throw new MultiRegistrationException(current);
                current = AllRooms.SingleOrDefault(x => x.ID.Equals(gamePin));
                if (current == null) throw new RoomNotExistException();
                CheckAvailability(current);
                Navigation.PushAsync(new EntryGameRoomPage(gamePin));
            }
            catch(RoomFullException exception) {
                DisplayAlert("Demesio", exception.Message, "Gerai");
            }
            catch (RoomNotExistException exception)
            {
                DisplayAlert("Demesio", exception.Message, "Gerai");
            }
            catch (MultiRegistrationException exception) {
                RoomExistError(exception.CurrentRoom,exception.Message);
            }
 
        }
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                SelectMsg(EnteredRooms[e.ItemIndex]);
            }
            MyListView.SelectedItem = null;
            Console.WriteLine(" " + e.Item);
        }
        async void SelectMsg(Room selectedRoom)//cia exseption negalima panaudoti
        {
            bool answer = await DisplayAlert("Demesio", "Ar norite testi zaidima- " + selectedRoom.ID, "Taip", "Ne");
            if (answer == true) Console.WriteLine("Iveinu  ");
        }
        public void EntryGame(Room room)
        {
            // Navigation.PushAsync("Quest Page")- dar neturiu tokio pago.
        }
        async void RoomExistError(Room selectedRoom,string msg)
        {
            bool answer = await DisplayAlert("Demesio", msg, "Taip", "Ne");
            if (answer == true) EntryGame(selectedRoom);
        }
        public void CheckAvailability(Room selectedRoom)
        {
            if (selectedRoom.ParticipantIDs.Count >= selectedRoom.RoomSize) throw new RoomFullException();
        }
    }
}
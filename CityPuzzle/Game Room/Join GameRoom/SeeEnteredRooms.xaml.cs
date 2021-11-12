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
            Console.WriteLine("Naikinu dideli loadinga--------------->");
            Thread tReadRooms = new Thread(() => { AllRooms = Sql.ReadRooms(); });
            Thread tReadUsers = new Thread(() => { AllUsers = Sql.ReadUsers(); });
            tReadRooms.Start();
            tReadUsers.Start();
            while (tReadRooms.IsAlive || tReadUsers.IsAlive) Thread.Sleep(300);
            ChangeView(delegate () { LoadingGrid.IsVisible = false; MainStack.IsVisible = true; });//dar vienas anonimas
            Task<List<Room>> treadFindRooms = new Task<List<Room>>(() => { return GetData(); });
            treadFindRooms.Start();
            Task.WaitAll();
            if (treadFindRooms.Result == null) ChangeView(delegate () { NoRooms.IsVisible = true;}); 
            else
            {
                ChangeView(delegate () {
                    MyListView.ItemsSource = treadFindRooms.Result;
                    MyListView.IsVisible = true;
                });
            }
            ChangeView(delegate () { LoadingSmallGrid.IsVisible = false; });
        }
        public static List<Room> GetData()
        {
            List<Room> findedRooms = new List<Room>();
            Task<List<string>> treadFind = new Task<List<string>>(
              () =>
              {
                  return Sql.FindParticipantRoomsIDs(App.CurrentUser.ID);
              });
            treadFind.Start();
            Task.WaitAll();
            foreach (string pin in treadFind.Result)
            {
                findedRooms.Add(AllRooms.SingleOrDefault(x => x.ID == pin));
            }
            return findedRooms;
        }
        void Sign_Click(object sender, EventArgs e)
        {
            string gamePin = GamePin.Text;
            Navigation.PushAsync(new EntryGameRoomPage(gamePin));

        }
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
    ((ListView)sender).SelectedItem = null;
            Console.WriteLine(" " + e.Item);
        }
    }
}
using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompliteCreating : ContentPage
    {
        public static int DefaultSize = 20;
        public static int[] Size = { 10, 20, 50, 80, 100 };
        public CompliteCreating()
        {
            InitializeComponent();
            MyListView.ItemsSource = CreateGamePage.NewRoom.Value.Tasks;
            idplace.Text = "Game pin: " + CreateGamePage.NewRoom.Value.Id;
            picker.ItemsSource = Size;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            ((ListView)sender).SelectedItem = null;
        }

        void Save_Click(object sender, EventArgs e)
        {
            //ADD SIZE SAVE
            if (picker.SelectedIndex == -1) CreateGamePage.NewRoom.Value.RoomSize = DefaultSize;
            else CreateGamePage.NewRoom.Value.RoomSize= Size[picker.SelectedIndex];
            Thread save_thread = new Thread(new ThreadStart(Save_Room));
            save_thread.Start();
            var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            foreach (var page in existingPages)
            {
                if (existingPages.Count == 2) break;
                if(existingPages.Count != stackSize) Navigation.RemovePage(page);
            }
            Navigation.PushAsync(new GameEntryPage());


        }
        public void Save_Room()
        {
            Sql.SaveRoom(CreateGamePage.NewRoom);
        }


    }
}

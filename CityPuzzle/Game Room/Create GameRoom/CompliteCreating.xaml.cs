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
        int CheckedSize = 20;
        enum Size
        {
            Huge = 100,
            Big = 50,
            Medium = 20,
            Small = 10
        }
        public CompliteCreating()
        {
            InitializeComponent();
            MyListView.ItemsSource = CreateGamePage.NewRoom.Value.Tasks;
            idplace.Text = "Game pin: " + CreateGamePage.NewRoom.Value.ID;
            picker.ItemsSource=new List<string>()
                    {
                        Size.Huge.ToString() + " - " + (int)Size.Huge,
                        Size.Big.ToString() + " = " + (int)Size.Big,
                        Size.Medium.ToString() + " = " + (int)Size.Medium,
                        Size.Small.ToString() + " = " + (int)Size.Small
                    };
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
            CreateGamePage.NewRoom.Value.RoomSize=picker.SelectedIndex;
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
            while (CreateGamePage.Data_collector_thread.IsAlive)
            {
                Thread.Sleep(500); //laukiu kada susikurs tinkamas gamepinas
            }
            Sql.SaveRoom(CreateGamePage.NewRoom);
        }


    }
}

using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompliteCreating : ContentPage
    {
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
            MyListView.ItemsSource = CreateGamePage.NewRoom.Tasks;
            idplace.Text = "Game pin: " + CreateGamePage.NewRoom.ID;
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
            using (SQLiteConnection conn = new SQLiteConnection(App.GamePath))
            {
                conn.CreateTable<Room>();
                int rowsAdded = conn.Insert(CreateGamePage.NewRoom);
            };
            var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            foreach (var page in existingPages)
            {
                if (existingPages.Count == 2) break;
                if(existingPages.Count != stackSize) Navigation.RemovePage(page);
            }
            Navigation.PushAsync(new GameEntryPage());


        }
    }
}

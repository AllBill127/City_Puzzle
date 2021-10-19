using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {
        public struct Spot
        {
            //int place;
            public string username { get; set; }
            public int score { get; set; }
        }

        public LeaderboardPage()
        {
            InitializeComponent();

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Puzzle>();
                List<User> top10 = conn.Table<User>().ToList().Top10();

                ObservableCollection<Spot> spots = new ObservableCollection<Spot>();
                foreach (var spot in top10)
                {
                    spots.Add(new Spot
                    {
                        username = spot.UserName, 
                        score = spot.QuestsCompleted.Count()
                    }
                    );
                }

                Leaderboard.ItemsSource = spots;
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}

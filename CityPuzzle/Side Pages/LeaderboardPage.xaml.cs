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
using static CityPuzzle.Classes.Structs;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {
        public LeaderboardPage()
        {
            InitializeComponent();

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Puzzle>();
                    // Form a top10 list with specified comparer and cast items in to new format with index
                List<UserInfo> top10 = conn.Table<User>().ToList().
                    Top10Cast(new PointsComparer(), (user, index) => new UserInfo
                    {
                        Username = user.UserName,
                        Score = user.QuestsCompleted.Count,
                        Index = index
                    });

                ObservableCollection<UserInfo> leaderboard = new ObservableCollection<UserInfo>(top10);

                Leaderboard.ItemsSource = leaderboard;
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

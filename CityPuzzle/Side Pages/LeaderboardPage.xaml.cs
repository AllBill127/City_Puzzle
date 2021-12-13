using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CityPuzzle.Classes.Structs;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {
        LeaderboardLogic lbLogic = new LeaderboardLogic();

        public LeaderboardPage()
        {
            InitializeComponent();

            lbLogic.OnPageChange += UpdatePage;

            lbLogic.ChangePage(true);
        }

        private void UpdatePage(object sender, LeaderboardLogic.OnPageChangeEventArgs e)
        {
            pageNumber.Text = "Page " + e.PageNr.ToString();
            ObservableCollection<UserInfo> leaderboard = new ObservableCollection<UserInfo>(e.PageList);

            Leaderboard.ItemsSource = leaderboard;
        }

        private void PrevButton_Clicked(object sender, EventArgs e)
        {
            lbLogic.ChangePage(false);
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            lbLogic.ChangePage(true);
        }

        private List<UserInfo> GetTop10()
        {
            // Form a top10 list with specified comparer and cast items in to new format with index
            List<UserInfo> top10 = Sql.ReadUsers().Top10Cast(new PointsComparer(), (user, index) => new UserInfo
            {
                Username = user.UserName,
                Score = user.QuestsCompleted.Count,
                Index = index
            });

            return top10;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            return;
        }
    }
}

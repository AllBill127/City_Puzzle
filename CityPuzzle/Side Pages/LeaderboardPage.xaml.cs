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
        private List<UserInfo> lbUsers = null;
        private int pageNr = 1;
        private IEnumerable<UserInfo> pageList = null;

        public LeaderboardPage()
        {
            InitializeComponent();

            // Form a sorted list of user info items for leaderboard
            lbUsers = Sql.ReadUsers().CastToLeaderboard(new PointsComparer(), (user, index) => new UserInfo
            {
                Username = user.UserName,
                Score = user.QuestsCompleted.Count,
                Index = index
            });

            pageNumber.Text = "Page " + pageNr.ToString();
            pageList = lbUsers.Skip(10).Take(10);
            ObservableCollection<UserInfo> leaderboard = new ObservableCollection<UserInfo>(pageList);

            Leaderboard.ItemsSource = leaderboard;
        }

        private void PrevButton_Clicked(object sender, EventArgs e)
        {
            if (pageNr > 1)
            {
                pageNr--;
                pageNumber.Text = "Page " + pageNr.ToString();
                pageList = lbUsers.Skip((pageNr - 1) * 10).Take(10);
                ObservableCollection<UserInfo> prevPage = new ObservableCollection<UserInfo>(pageList);

                Leaderboard.ItemsSource = prevPage;
            }
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            if (pageNr < lbUsers.Count / 10)
            {
                pageNr++;
                pageNumber.Text = "Page " + pageNr.ToString();
                pageList = lbUsers.Skip((pageNr - 1) * 10).Take(10);
                ObservableCollection<UserInfo> nextPage = new ObservableCollection<UserInfo>(pageList);

                Leaderboard.ItemsSource = nextPage;
            }
            else if (pageNr == lbUsers.Count / 10)
            {
                pageNr++;
                pageNumber.Text = "Page " + pageNr.ToString();
                pageList = lbUsers.Skip((pageNr - 1) * 10).Take(lbUsers.Count % 10);
                ObservableCollection<UserInfo> nextPage = new ObservableCollection<UserInfo>(pageList);

                Leaderboard.ItemsSource = nextPage;
            }
        }

        private List<UserInfo> GetTop10 ()
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
    }
}

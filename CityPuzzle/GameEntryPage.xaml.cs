using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using CityPuzzle.Classes;
using CityPuzzle.Game_Room.Join_GameRoom;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameEntryPage : ContentPage
    {
        public GameEntryPage()
        {
            InitializeComponent();
        }
        void StartButton_click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new QuestPage());

        }
        void Create_Click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new CreateGamePage());

        }
        void Add_Click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new AddObjectPage());

        }

        void Entry_Click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new SeeEnteredRooms());

        }

        void Button_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            string theTextFile = wc.DownloadString("https://onedrive.live.com/?authkey=%21AFs2jqf6YPPLw7k&cid=E3EB53E039BE7E4D&id=E3EB53E039BE7E4D%21540&parId=root&o=OneUp");
            Console.WriteLine(theTextFile);
        }

        private void Leaderboard_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LeaderboardPage());
        }
        private void Settings_Clicked(object sender, EventArgs e)
        {
            App.CurrentUser = null;
            Sql.SaveCurrentUser(new User("", ""));
            var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            foreach (var page in existingPages)
            {
                if (existingPages.Count == 2) break;
                if (existingPages.Count != stackSize) Navigation.RemovePage(page);
            }
            Navigation.PopAsync();
        }
    }
}
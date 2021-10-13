using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompliteCreating : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public CompliteCreating()
        {
            InitializeComponent();
            MyListView.ItemsSource = CreateGamePage.newroom.Tasks;
            idplace.Text = "Game pin: "+CreateGamePage.newroom.ID;

           
        }
        
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

           

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.GamePath))
            {
                conn.CreateTable<Room>();
                int rowsAdded = conn.Insert(CreateGamePage.newroom);
            };

        }
    }
}

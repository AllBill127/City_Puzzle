using CityPuzzle.Classes;
using SQLite;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompliteCreating : ContentPage
    {
        public CompliteCreating()
        {
            InitializeComponent();
            MyListView.ItemsSource = CreateGamePage.NewRoom.Tasks;
            idplace.Text = "Game pin: " + CreateGamePage.NewRoom.ID;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.GamePath))
            {
                conn.CreateTable<Room>();
                int rowsAdded = conn.Insert(CreateGamePage.NewRoom);
            };

        }}
}

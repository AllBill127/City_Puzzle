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
    public partial class CompleteRoomPage : ContentPage
    {
        public static int DefaultSize = 20;
        public static int[] Size = { 10, 20, 50, 80, 100 };

        public CompleteRoomPage()
        {
            InitializeComponent();

            puzzleListView.ItemsSource = new ObservableCollection<Puzzle>(CreateRoomPage.RoomPuzzles);
            roomPin.Text = "Game pin: " + CreateRoomPage.NewRoom.Value.RoomPin;
            sizePicker.ItemsSource = Size;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            ((ListView)sender).SelectedItem = null;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (sizePicker.SelectedIndex == -1)
                CreateRoomPage.NewRoom.Value.RoomSize = DefaultSize;
            else
                CreateRoomPage.NewRoom.Value.RoomSize = Size[sizePicker.SelectedIndex];

            Thread save_thread = new Thread(new ThreadStart(() =>
            {
                CreateRoomPage.NewRoom.Value.Save(CreateRoomPage.PuzzleIds);
            }));
            save_thread.Start();
            save_thread.Join();

            // TO DO: 
            // use static navigation function which is to be added later
            var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            foreach (var page in existingPages)
            {
                if (existingPages.Count == 2)
                    break;
                if (existingPages.Count != stackSize)
                    Navigation.RemovePage(page);
            }

            Navigation.PushAsync(new GameMenuPage());
        }
    }
}

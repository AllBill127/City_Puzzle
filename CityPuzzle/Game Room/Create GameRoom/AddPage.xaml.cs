using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPage : ContentPage
    {
        public List<Lazy<Puzzle>> AllPuzzles;
        public static int Nr = 0;
        public AddPage()
        {
            Nr = 0;
<<<<<<< HEAD:CityPuzzle/Game Room/AddPage.xaml.cs
            CreateGamePage.NewRoom.Value.Tasks = new List<Lazy<Puzzle>>(); 
=======
            CreateGamePage.NewRoom.Value.Tasks= new List<Lazy<Puzzle>>(); 
>>>>>>> parent of 33c7fbd (Merge pull request #46 from AllBill127/revert-44-GameRoom_Update):CityPuzzle/Game Room/Create GameRoom/AddPage.xaml.cs
            InitializeComponent();
            AllPuzzles = Sql.ReadPuzzles();
            if (AllPuzzles.Count == 0)
            {
                EmptyListError();
            }
            else
            {
                Show();
            }
        }
        async void EmptyListError()
        {
            await DisplayAlert("Error", "Nepavyksta aptikti delioniu.", "OK");
        }
        void Add_puzzle(object sender, EventArgs e)
        {
            CreateGamePage.NewRoom.Value.Tasks.Add(AllPuzzles[Nr]);
            Next_puzzle(sender, e);
        }
        void Next_puzzle(object sender, EventArgs e)
        {
            if (Nr == AllPuzzles.Count - 1)
            {
                Navigation.PopAsync();
            }
            else
            {
                Nr += 1;
                Show();
            }
        }

        public void Show()
        {
            Console.WriteLine("ciadaejo");
            PuzzleName.Text = AllPuzzles[Nr].Value.Name;
            PuzzleImg.Source = AllPuzzles[Nr].Value.ImgAdress;
            PuzzleInfo.Text = AllPuzzles[Nr].Value.About;
        }
    }
}
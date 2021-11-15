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
            CreateGamePage.NewRoom.Value.Tasks = new List<Lazy<Puzzle>>(); 
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
            CreateGamePage.NewRoom.Tasks.Add(AllPuzzles[Nr]);
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
            PuzzleName.Text = AllPuzzles[Nr].Value.Name;
            PuzzleImg.Source = AllPuzzles[Nr].Value.ImgAdress;
            PuzzleInfo.Text = AllPuzzles[Nr].Value.About;
        }
    }
}
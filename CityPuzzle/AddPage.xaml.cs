using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPage : ContentPage
    {
        public List<Puzzle> AllPuzzles;
        public static int Nr = 0;
        public AddPage()
        {
            Nr = 0;
            CreateGamePage.newroom.Tasks.Clear();
            InitializeComponent();
            using (SQLiteConnection conn = new SQLiteConnection(App.ObjectPath))
            {
                conn.CreateTable<Puzzle>();
                AllPuzzles = conn.Table<Puzzle>().ToList();
            }
            if (AllPuzzles.Count == 0) EmptyListError();
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
           
            CreateGamePage.newroom.Tasks.Add(AllPuzzles[Nr]);
            Console.WriteLine("Pridejau uzduoti- " + AllPuzzles[Nr].Name);
            Next_puzzle(sender,e);


        }
        void Next_puzzle(object sender, EventArgs e)
        {
            if (Nr == AllPuzzles.Count-1)
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
            
            PuzzleName.Text = AllPuzzles[Nr].Name;
            PuzzleImg.Source = AllPuzzles[Nr].ImgAdress;
            PuzzleInfo.Text = AllPuzzles[Nr].About;
        }
    }
}
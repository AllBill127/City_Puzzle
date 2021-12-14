using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle.Side_Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PuzzleScoresPage : ContentPage
    {
        private List<Puzzle> allPuzzles = Sql.ReadPuzzles();
        private List<CompletedPuzzle2> completedPuzzles = ((User)(from user in Sql.ReadUsers() where user.ID == App.CurrentUser.ID select user)).CompletedPuzzles;

        public PuzzleScoresPage()
        {
            InitializeComponent();

            var groupedPuzzles = completedPuzzles.GroupBy(
                puzzle => puzzle.PuzzleId,
                puzzle => puzzle.Score,
                (key, group) => new { PuzzleId = key, Scores = group.ToList() });

            var puzzleScores = allPuzzles.Join(
                groupedPuzzles,
                puzzle => puzzle.ID,
                gPuzzle => gPuzzle.PuzzleId,
                (puzzle, gPuzzle) => new
                {
                    PuzzleName = puzzle.Name,
                    ImgAdress = puzzle.ImgAdress,
                    Score = gPuzzle.Scores.Sum()
                });

            PuzzleScores.ItemsSource = puzzleScores;
        }

        private void Handle_ItemTapped(object sender, EventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            return;
        }
    }
}
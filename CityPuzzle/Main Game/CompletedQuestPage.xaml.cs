using CityPuzzle.Classes;
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
    public partial class CompletedQuestPage : ContentPage
    {
        private readonly List<Puzzle> questsList;

        public CompletedQuestPage(Puzzle quest, List<Puzzle> questsList, int score)
        {
            InitializeComponent();
            this.questsList = questsList;

            var completedPuzzle = new CompletedPuzzle() { UserId = App.CurrentUser.ID, PuzzleId = quest.ID };
            var completedPuzzle2 = new CompletedPuzzle2() { UserId = App.CurrentUser.ID, PuzzleId = quest.ID, Score = score };
            completedPuzzle2.Save();
            completedPuzzle.Save();

            completedName.Text = quest.Name;
            completedInfo.Text = quest.About;
            completedImg.Source = quest.ImgAdress;
            completedScore.Text = "Score: " + score.ToString();
        }

        protected override void OnDisappearing()// GAL reiktu tam klase nauja sukurt (Navigation)
        {
            //TO DO: Add navigation deletion
            /*var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            Navigation.RemovePage(existingPages[existingPages.Count - 1]);*/
        }

        protected override void OnAppearing()
        {
            //TO DO: Add navigation deletion
            /*var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            //Navigation.RemovePage(existingPages[existingPages.Count - 2]);*/
        }

        private void NewPuzzle_clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestPage(questsList));
        }
    }
}
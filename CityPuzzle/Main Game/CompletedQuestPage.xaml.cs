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

        public CompletedQuestPage(Puzzle quest, List<Puzzle> questsList)
        {
            InitializeComponent();

            this.questsList = questsList;
            Sql.SaveCompletedPuzzle(quest);
            completedName.Text = quest.Name;
            completedInfo.Text = quest.About;
            completedImg.Source = quest.ImgAdress;
        }
        private void NewPuzzle_clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestPage(questsList));
        }
        protected override void OnDisappearing()// GAL reiktu tam klase nauja sukurt (Navigation)
        {
            var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            Navigation.RemovePage(existingPages[existingPages.Count - 1]);
        }
        protected override void OnAppearing()
        {
            var existingPages = Navigation.NavigationStack.ToList();
            int stackSize = existingPages.Count;
            Navigation.RemovePage(existingPages[existingPages.Count - 2]);
        }
    }
}
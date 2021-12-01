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
    public partial class ComplitedPage : ContentPage
    {
        private readonly List<Lazy<Puzzle>> questsList;

        public ComplitedPage(Puzzle quest, List<Lazy<Puzzle>> questsList)
        {
            InitializeComponent();

            this.questsList = questsList;
            Sql.SaveComplitedTask(quest);
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
            existingPages=NavigationHandler.CleanOnePage(existingPages, 1);
        }
        protected override void OnAppearing()
        {
            var existingPages = Navigation.NavigationStack.ToList();
            existingPages=NavigationHandler.CleanOnePage(existingPages, 2);
        }
    }
}
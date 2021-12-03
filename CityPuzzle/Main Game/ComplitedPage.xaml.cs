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
        private readonly List<Puzzle> questsList;

        public ComplitedPage(Puzzle quest, List<Puzzle> questsList)
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
    }
}
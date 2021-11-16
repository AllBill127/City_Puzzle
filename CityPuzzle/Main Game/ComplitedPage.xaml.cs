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
        private List<Lazy<Puzzle>> questsList;

        public ComplitedPage(Puzzle quest, List<Lazy<Puzzle>> questsList)
        {
            InitializeComponent();

            this.questsList = questsList;
            Sql.SaveComplitedTask(quest);
            Complitedname.Text = quest.Name;
            Complitedinfo.Text = quest.About;
            ComplitedImg.Source = quest.ImgAdress;
        }
        void NewPuzzle(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestPage(questsList));
        }
    }
}
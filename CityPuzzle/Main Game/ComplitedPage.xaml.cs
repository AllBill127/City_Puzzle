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
        public ComplitedPage(Puzzle quest)
        {
            InitializeComponent();
            Sql.SaveComplitedTask(quest);
            Complitedname.Text = quest.Name;
            Complitedinfo.Text = quest.About;
            ComplitedImg.Source = quest.ImgAdress;
        }
        void NewPuzzle(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestPage());
        }
    }
}
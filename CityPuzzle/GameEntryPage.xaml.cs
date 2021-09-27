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
    public partial class GameEntryPage : ContentPage
    {
        public GameEntryPage()
        {
            InitializeComponent(); }
        void StartButton_click(object sender, EventArgs e)
        {
           
            Navigation.PushAsync(new QuestPage());

        }
        void Add_Click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new AddObjectPage());

        }
        void Look_Click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new LookPage());

        }
    }
}
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
    public partial class LookPage : ContentPage
    {
        public LookPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

              ObjectsListView.ItemsSource = Sql.ReadPuzzles();

            }
        
        void deleate_click(object sender, EventArgs e)
        {
            base.OnAppearing();
            //add deleate
        }
    }
}
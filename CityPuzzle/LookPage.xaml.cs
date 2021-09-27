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
            using (SQLiteConnection conn = new SQLiteConnection(App.ObjectPath))
            {
                conn.CreateTable<Puzzle>();
                var obj = conn.Table<Puzzle>().ToList();
                ObjectsListView.ItemsSource = obj;

            }
        }
    }
}
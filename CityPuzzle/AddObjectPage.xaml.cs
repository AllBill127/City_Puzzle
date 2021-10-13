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
    public partial class AddObjectPage : ContentPage
    {
        public AddObjectPage()
        {
            InitializeComponent();

        }
        void Save_Click(object sender, EventArgs e)
        {
            Puzzle obj = new Puzzle()
            {
                About = ObjectAbout.Text,
                Quest = ObjectQuest.Text,
                Name = ObjectName.Text,
                ImgAdress = ObjectImg.Text,
                Latitude= Convert.ToDouble(ObjectX.Text),
                Longitude= Convert.ToDouble(ObjectY.Text),

            };
            using (SQLiteConnection conn = new SQLiteConnection(App.ObjectPath))
            {
                conn.CreateTable<Puzzle>();
                int rowsAdded = conn.Insert(obj);
            };

            Navigation.PopAsync();
        }

    }
}
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
        private void SaveObject_Clicked(object sender, EventArgs e)
        {
            if (objectAbout.Text != null || objectAbout.Text != "" ||
                objectQuest.Text != null || objectQuest.Text != "" ||
                objectName.Text != null || objectName.Text != "" ||
                objectImg.Text != null || objectImg.Text != "" ||
                objectX.Text != null || objectX.Text != "" ||
                objectY.Text != null || objectY.Text != "")
            {
                Puzzle obj = new Puzzle()
                {
                    About = objectAbout.Text,
                    Quest = objectQuest.Text,
                    Name = objectName.Text,
                    ImgAdress = objectImg.Text,
                    Latitude = Convert.ToDouble(objectX.Text),
                    Longitude = Convert.ToDouble(objectY.Text),
                };
                obj.Save();

                Navigation.PopAsync();
            }
        }

    }
}
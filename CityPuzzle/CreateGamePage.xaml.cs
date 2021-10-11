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
    public partial class CreateGamePage : ContentPage
    {
        public Admin newAdmin;
        public Room newroom;

        public CreateGamePage()
        {
            String roomId = "kambarys" + App.CurrentUser.ID;// Ateityje padaryti random su patikrinimu arba ne;

            newAdmin = new Admin(roomId, App.CurrentUser);

            newroom = new Room(roomId);



            InitializeComponent();
        }

        void AddObj_click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new AddPage(newroom));

        }





    }
}
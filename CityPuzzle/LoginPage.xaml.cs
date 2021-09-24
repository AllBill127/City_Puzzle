using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CityPuzzle.Classes;

namespace CityPuzzle
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

        }
        void Login_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Prisijungia");
            if (User.CheckPassword(Vartotojo_vardas.Text, Slaptazodis.Text)) Navigation.PushAsync(new MapPage());
            else
            {
                Vartotojo_vardas.Text = "";
                Slaptazodis.Text = "";
                OnAlertYesNoClicked();
            }
        }
        async void OnAlertYesNoClicked()
        {
            await DisplayAlert("Error", "Naudotojas su tokiu prisijungimo vardu nerastas, arba neteisingai įvedėte slaptažodį. Pasitikrinkite prisijungimo vardą ir bandykite dar kartą.", "OK");

        }
        void Sign_Click(object sender, EventArgs e)
        {

            Navigation.PushAsync(new SignUpPage());

        }
    }
}

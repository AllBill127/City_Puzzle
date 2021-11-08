using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CityPuzzle.Classes;
using System.Threading;
using System.IO;

namespace CityPuzzle
{
    public partial class LoginPage : ContentPage
    {
        
        public LoginPage()
        {

            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Vartotojo_vardas.Text = "";
            Slaptazodis.Text = "";
            while (App.FilePath == null)
            {
                Thread.Sleep(500);
                Console.WriteLine("miegu");
            }
            SimpleUser current = Sql.GetCurrentUser();
            if (current != null && User.CheckHachedPassword(current.UserName, current.HashedPass)) Navigation.PushAsync(new GameEntryPage());
        }
        void Login_Click(object sender, EventArgs e)
        {
            if (User.CheckPassword(Vartotojo_vardas.Text, Slaptazodis.Text)) Navigation.PushAsync(new GameEntryPage());
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
            Slaptazodis.Text = "";
            Navigation.PopToRootAsync();

        }
    }
}

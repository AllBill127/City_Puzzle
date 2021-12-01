using CityPuzzle.Classes;
using System;
using Xamarin.Forms;

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
            SimpleUser current = Sql.GetCurrentUser();
            var tempUser = new User(new UserVerifier());
            if (current != null && tempUser.CheckHachedPassword(current.UserName, current.HashedPass))
            {
                Navigation.PushAsync(new GameEntryPage());
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            var tempUser = new User(new UserVerifier());
            if (tempUser.CheckPassword(Vartotojo_vardas.Text, Slaptazodis.Text))
            {
                Navigation.PushAsync(new GameEntryPage());
            }
            else
            {
                Vartotojo_vardas.Text = "";
                Slaptazodis.Text = "";
                OnAlertYesNoClicked();
            }
        }

        private async void OnAlertYesNoClicked()
        {
            await DisplayAlert("Error", "Naudotojas su tokiu prisijungimo vardu nerastas, arba neteisingai įvedėte slaptažodį. Pasitikrinkite prisijungimo vardą ir bandykite dar kartą.", "OK");
        }
        void Sign_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }
    }
}

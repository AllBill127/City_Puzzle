using CityPuzzle.Classes;
using System;
using Xamarin.Forms;

namespace CityPuzzle
{
    public partial class LoginPage : ContentPage
    {
        LoginPageLogic loginLogic = new LoginPageLogic();
        public LoginPage()
        {
            InitializeComponent();
            loginLogic.OnUserIsLogedIn += LoadGameMenu;
            loginLogic.OnUserNotFound += UserNotFoundAlert;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            username.Text = "";
            password.Text = "";
            loginLogic.LogIn(null, null);
        }

        private void LoadGameMenu(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GameEntryPage());
        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            loginLogic.LogIn(username.Text, password.Text);
        }

        private async void UserNotFoundAlert(object sender, EventArgs e)
        {
            username.Text = "";
            password.Text = "";
            await DisplayAlert("Error", "Naudotojas su tokiu prisijungimo vardu nerastas, arba neteisingai įvedėte slaptažodį. Pasitikrinkite prisijungimo vardą ir bandykite dar kartą.", "OK");
        }
        private void SignIn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }
    }
}

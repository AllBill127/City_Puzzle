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
<<<<<<< HEAD

            SimpleUser current = Sql.GetCurrentUser();
            var tempUser = new User(new UserVerifier());
            if (current != null && tempUser.CheckHachedPassword(current.UserName, current.HashedPass)) Navigation.PushAsync(new GameEntryPage());
=======
            SimpleUser current_ = Sql.GetCurrentUser();
            if (current_ != null && User.CheckHachedPassword(current_.UserName, current_.HashedPass)) Navigation.PushAsync(new GameEntryPage());  
>>>>>>> parent of 33c7fbd (Merge pull request #46 from AllBill127/revert-44-GameRoom_Update)
        }
        void Login_Click(object sender, EventArgs e)
        {
            var tempUser = new User(new UserVerifier());
            if (tempUser.CheckPassword(Vartotojo_vardas.Text, Slaptazodis.Text)) Navigation.PushAsync(new GameEntryPage());
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

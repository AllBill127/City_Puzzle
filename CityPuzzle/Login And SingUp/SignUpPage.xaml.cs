using CityPuzzle.Classes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void Registration_Click(object sender, EventArgs e)
        {
            try
            {
                SignUpPageLogic logic = new SignUpPageLogic();

                List<Entry> fields = new List<Entry>();
                fields.Add(usernameEntry);
                fields.Add(passEntry);
                fields.Add(nameEntry);
                fields.Add(lastnameEntry);
                fields.Add(emailEntry);
                logic.Validation(fields);

                logic.CreateUser(usernameEntry.Text, passEntry.Text, nameEntry.Text, lastnameEntry.Text, emailEntry.Text, distEntry.Text);

                Navigation.PopAsync();
            }
            catch (BadInputdException exception)
            {
                ErrorAllert(exception.Message);
                exception.Field.BackgroundColor = Color.Orange;
                exception.Field.Text = "Neteisingai įvedėt šį lauką";
            }
            catch (EmptyInputdException exception)
            {
                ErrorAllert(exception.Message);
                exception.Field.BackgroundColor = Color.Orange;
                exception.Field.Text = "Prašom užpildyti šį lauką";
            }
            passEntry.Text = "";
        }

        async void ErrorAllert(string message)
        {
            await DisplayAlert("Klaida", message, "Gerai");
        }
    }
}

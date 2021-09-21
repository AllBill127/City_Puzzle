using CityPuzzle.Classes;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }
        void Registration_Click(object sender, EventArgs e)
        {
            try
            {
                if (nameEntry.Text.Length < 1 || lastnameEntry.Text.Length < 1 || usernameEntry.Text.Length < 1)
                {

                    MissInfoError();
                }
                else if (!User.CheckUser(usernameEntry.Text))
                {
                    usernameEntry.Text = "";
                    passEntry.Text = "";
                    SignErrorAllert();
                }
                if (usernameEntry.Text.Length < 5)
                {
                    usernameEntry.Text = "";
                    passEntry.Text = "";
                    SignErrorPassAllert();
                }
                else
                {
                    User user = new User()
                    {
                        Name = nameEntry.Text,
                        LastName = lastnameEntry.Text,
                        UserName = usernameEntry.Text,
                        Pass = passEntry.Text,

                    };
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.CreateTable<User>();
                        int rowsAdded = conn.Insert(user);
                    };
                    Navigation.PushAsync(new LoginPage());

                }
            }
            catch (NullReferenceException ex)
            {
                MissInfoError();
            }
        }
        async void SignErrorAllert()
        {
            await DisplayAlert("Error", "Naudotojas su tokiu prisijungimo vardu užimtas. Pakeiskite prisijungimo vardą ir bandykite dar kartą.", "OK");

        }
        async void SignErrorPassAllert()
        {
            await DisplayAlert("Error", "Naudotojo slaptažodis per trumpas. Pakeiskite slaptažodį ir bandykite dar kartą.", "OK");

        }
        async void MissInfoError()
        {
            await DisplayAlert("Error", "Nepakanka duomenų.", "OK");

        }
    }
}
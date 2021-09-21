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
            if (!User.CheckUser(usernameEntry.Text))
            {
                usernameEntry.Text = "";
                passEntry.Text = "";
                SignErrorAllert();
            }
            else
            {

                Console.WriteLine("Registruoja");
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
        async void SignErrorAllert()
        {
            await DisplayAlert("Error", "Naudotojas su tokiu prisijungimo vardu uŽimtas. Pakeiskite prisijungimo vardą ir bandykite dar kartą.", "OK");

        }
    }
}
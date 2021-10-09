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
                if (String.IsNullOrWhiteSpace(nameEntry.Text) || String.IsNullOrWhiteSpace(lastnameEntry.Text) || String.IsNullOrWhiteSpace(usernameEntry.Text))
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
                    User user;

                    if (distEntry != null)
                    {
                        user = CreateUser(name: nameEntry.Text, userName: usernameEntry.Text, lastName: lastnameEntry.Text, password: User.PassToHash(passEntry.Text), maxDist: double.Parse(distEntry.Text));
                    }
                    else
                    {
                        user = CreateUser(name: nameEntry.Text, userName: usernameEntry.Text, lastName: lastnameEntry.Text, password: User.PassToHash(passEntry.Text));
                    }

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.CreateTable<User>();
                        int rowsAdded = conn.Insert(user);
                    };

                    Navigation.PopAsync();
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
        private User CreateUser(string userName, string name, string lastName, string password, double maxDist = 3)
        {
            User user = new User()
            {
                Name = name,
                LastName = lastName,
                UserName = userName,
                Pass = password,
                maxQuestDistance = maxDist
            };

            return user;
        }
    }
}
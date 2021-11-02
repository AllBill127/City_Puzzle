using CityPuzzle.Classes;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }
 
        public delegate Regex validator(string regex);
        void Registration_Click(object sender, System.EventArgs e)
        {
            validator pattern = delegate (string regex)
            {
                string x = regex;

                return new Regex(x, RegexOptions.IgnoreCase);
            };

            //For username
            Regex Valid_Username = pattern("^(?=.{6,12}$)(?![_.])(?!.*[_.]{2})[a-z0-9._]+(?<![_.])$");

            if (usernameEntry.Text == null)
            {
                var result = DisplayAlert("Invalid", "Username cannot be empty!", "Yes", "Cancel");
                return;
            }
            else if (Valid_Username.IsMatch(usernameEntry.Text) != true)
            {
                var result = DisplayAlert("Invalid", "Username length is between 6 to 12", "Yes", "Cancel");
                return;
            }
            
            //For password
            Regex Valid_Password = pattern("(?!^[0-9]*$)(?!^[a-z]*$)^([a-z0-9]{8,15})$");

            if (passEntry.Text == null)
            {
                var result = DisplayAlert("Invalid", "Password cannot be empty!", "Yes", "Cancel");
                return;
            }
            else if (Valid_Password.IsMatch(passEntry.Text) != true)
            {
                var result = DisplayAlert("Password must be atleast 8 to 15 characters.", "It must contain letters and numbers.", "Yes", "Cancel");
                return;
            }
            
            //Validating email address
            Regex Valid_Email = pattern(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$");

            if (emailEntry.Text == null)
            {
                var result = DisplayAlert("Invalid", "Email cannot be empty!", "Yes", "Cancel");
                return;
            }
            else if (Valid_Email.IsMatch(emailEntry.Text) != true)
            {
                var result = DisplayAlert("Invalid Email Address!", "Invalid", "Yes", "Cancel");
                return;
            }

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
                else if (usernameEntry.Text.Length < 5)
                {
                    usernameEntry.Text = "";
                    passEntry.Text = "";
                    SignErrorPassAllert();
                }
                else
                {
                    User user;

                    if (distEntry.Text != null)
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

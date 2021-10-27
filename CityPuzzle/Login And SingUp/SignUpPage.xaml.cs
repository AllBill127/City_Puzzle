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
 
        static Regex Valid_Username = StringNumber();
        static Regex Valid_Contact = NumbersOnly();
        static Regex Valid_Password = ValidPassword();
        static Regex Valid_Email = Email_Address();

        private static Regex StringNumber()
        {
            string StringAndNumber_Pattern = "^[a-zA-Z][a-zA-Z0-9]{5,11}";

            return new Regex(StringAndNumber_Pattern, RegexOptions.IgnoreCase);
        }

        //Method for validating email address
        private static Regex Email_Address()
        {
            string Email_Pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(Email_Pattern, RegexOptions.IgnoreCase);
        }

        private static Regex NumbersOnly()
        {
            string StringAndNumber_Pattern = "^[0-9]*$";

            return new Regex(StringAndNumber_Pattern, RegexOptions.IgnoreCase);
        }

        private static Regex ValidPassword()
        {
            string Password_Pattern = "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,15})$";

            return new Regex(Password_Pattern, RegexOptions.IgnoreCase);
        }
        void Registration_Click(object sender, EventArgs e)
        {
         //for username
            if (usernameEntry.Text == null)
            {
                var result = DisplayAlert("Invalid", "Username cannot be empty!", "Yes", "Cancel");
                return;
            }
            else if (Valid_Username.IsMatch(usernameEntry.Text))
            {
                var result = DisplayAlert("Invalid", "Length between 6 to 12", "Yes", "Cancel");
                return;
            }


            //for password
            if (passEntry.Text == null)
            {
                var result = DisplayAlert("Invalid", "Password cannot be empty!", "Yes", "Cancel");
                return;
            }
            else if (Valid_Password.IsMatch(passEntry.Text) != true)
            {
                var result = DisplayAlert("Password must be atleast 8 to 15 characters.", "It contains atleast one Upper case and numbers.", "Yes", "Cancel");
                return;
            }


            //for Email Address
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

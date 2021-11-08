using CityPuzzle.Classes;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;
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
 
        static Regex Valid_Username = StringNumber();
        static Regex Valid_Contact = NumbersOnly();
        static Regex Valid_Password = ValidPassword();
        static Regex Valid_Email = Email_Address();

        private static Regex StringNumber()
        {
            string StringAndNumber_Pattern = "^[a-zA-Z][a-zA-Z0-9]{5,11}";

            return new Regex(StringAndNumber_Pattern, RegexOptions.IgnoreCase);
        }


        public static void Validation(List<Entry> fields)
        {
            foreach (Entry field in fields)
            {
                if (String.IsNullOrWhiteSpace(field.Text)) throw new EmptyInputdException(field.Placeholder, field);

                switch (field.Placeholder)
                {
                    case "Vartotojo vardas":
                        if (!Valid_Username.IsMatch(field.Text)) throw new BadInputdException("nuo 6 iki 12 ilgio", field.Placeholder, field);
                        else if (!User.CheckUser(field.Text)) throw new BadInputdException("Naudotojas su tokiu prisijungimo vardu užimtas.Pakeiskite prisijungimo vardą ir bandykite dar kartą.", field);
                        break;
                    case "Slaptazodis":
                        if (!Valid_Password.IsMatch(field.Text)) throw new BadInputdException("Password must be atleast 8 to 15 characters and contains atleast one Upper case and numbers.", field);
                        break;
                    case "Pastas":
                        if (!Valid_Email.IsMatch(field.Text)) throw new BadInputdException("Invalid Email Address!", field);
                        break;
                    default:
                        break;
                }
            }

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
            
            try
            {
                List<Entry> fields = new List<Entry>();
                fields.Add(usernameEntry);
                fields.Add(passEntry);
                fields.Add(nameEntry);
                fields.Add(lastnameEntry);
                fields.Add(emailEntry);
                Validation(fields);
                User user;

                if (distEntry.Text != null)
                {
                    user = CreateUser(name: nameEntry.Text, userName: usernameEntry.Text, lastName: lastnameEntry.Text, password: User.PassToHash(passEntry.Text),email: emailEntry.Text, maxDist: double.Parse(distEntry.Text));
                }
                else
                {
                    user = CreateUser(name: nameEntry.Text, userName: usernameEntry.Text, lastName: lastnameEntry.Text, email: emailEntry.Text, password: User.PassToHash(passEntry.Text));
                }
                Sql.SaveUser(user);

                Navigation.PopAsync();

            }
            catch (BadInputdException exception)
            {
                ErrorAllert(exception.Message);
                exception.Field.BackgroundColor = Color.Orange;
                exception.Field.TextColor = Color.Red;
                exception.Field.Text = "Neteisingai įvedėt  šį lauką";
            }
            catch (EmptyInputdException exception)
            {

                ErrorAllert(exception.Message);
                exception.Field.BackgroundColor = Color.Red;
                exception.Field.Text = "Prašom užpildyti šį lauką ";

            }

            passEntry.Text = "";

        }

        async void ErrorAllert(string message)
        {
            await DisplayAlert("Klaida", message, "Gerai");

        }

        private User CreateUser(string userName, string name, string lastName, string password,string email, double maxDist = 3)
        {
            List<Lazy<Puzzle>> zero = new List<Lazy<Puzzle>>();
            User user = new User()
            {
                Name = name,
                LastName = lastName,
                UserName = userName,
                Pass = password,
                Email = email,
                MaxQuestDistance = maxDist,
                QuestsCompleted = zero
            };

            return user;
        }
    }
}

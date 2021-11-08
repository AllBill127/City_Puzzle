using CityPuzzle.Classes;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

        public delegate bool Validator(string text);

        public static void Validation(List<Entry> fields)
        {
            Validator validateUsername = new Validator(RegexDelegate.validUsername);
            Validator validatePassword = new Validator(RegexDelegate.validPassword);
            Validator validateEmail = new Validator(RegexDelegate.validEmail);

            foreach (Entry field in fields)
            {
                if (String.IsNullOrWhiteSpace(field.Text))
                    throw new EmptyInputdException(field.Placeholder, field);

                // field.Placeholder is defined in SignUpPage.xaml Entity propertie for text fields
                switch (field.Placeholder)
                {
                    case "Vartotojo vardas":
                        if (!validateUsername(field.Text)) 
                            throw new BadInputdException("nuo 6 iki 12 ilgio", field.Placeholder, field);
                        else if (!User.CheckUser(field.Text)) 
                            throw new BadInputdException("Naudotojas su tokiu prisijungimo vardu užimtas.Pakeiskite prisijungimo vardą ir bandykite dar kartą.", field);
                        break;
                    case "Slaptazodis":
                        if (!validatePassword(field.Text)) 
                            throw new BadInputdException("Password must be atleast 8 to 15 characters and contains atleast one Upper case and numbers.", field);
                        break;
                    case "Pastas":
                        if (!validateEmail(field.Text)) 
                            throw new BadInputdException("Invalid Email Address!", field);
                        break;
                    default:
                        break;
                }
            }

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
                    user = CreateUser(name: nameEntry.Text, userName: usernameEntry.Text, lastName: lastnameEntry.Text, password: User.PassToHash(passEntry.Text), email: emailEntry.Text, maxDist: double.Parse(distEntry.Text));
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

        private User CreateUser(string userName, string name, string lastName, string email, string password, double maxDist = 3)
        {
            User user = new User()
            {
                Name = name,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Pass = password,
                MaxQuestDistance = maxDist
            };

            return user;
        }
    }
}

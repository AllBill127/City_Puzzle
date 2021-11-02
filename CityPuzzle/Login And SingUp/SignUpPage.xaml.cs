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


        public static void Validation(string item,int called,string name)
        {
            if(String.IsNullOrWhiteSpace(item)) throw new EmptyInputdException(name);

            switch (called)
            {
                case 0:
                    break;
                case 1:
                    if (!Valid_Username.IsMatch(item)) throw new BadInputdException("nuo 6 iki 12 ilgio", name);
                    else if (!User.CheckUser(item)) throw new BadInputdException("Naudotojas su tokiu prisijungimo vardu užimtas.Pakeiskite prisijungimo vardą ir bandykite dar kartą.");
                    break;
                case 2:
                    if (!Valid_Password.IsMatch(item)) throw new BadInputdException("Password must be atleast 8 to 15 characters and contains atleast one Upper case and numbers.");
                    break;
                case 3:
                    if (!Valid_Email.IsMatch(item)) throw new BadInputdException("Invalid Email Address!");
                    break;
                default:
                    throw new BadInputdException("No validation case fuond in ", name);
                    break;
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
            //for username
            
            try
            {
                Validation(usernameEntry.Text, 1, "vartotojo varda");
                Validation(passEntry.Text, 2, "Slaptazodzio");
                Validation(emailEntry.Text, 3, "Emailo");
                Validation(nameEntry.Text, 0, "Vardo");
                Validation(lastnameEntry.Text, 0, "Pavardes");
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
            }
            catch (EmptyInputdException exception)
            {

                ErrorAllert(exception.Message);
            }
            finally
            {
                usernameEntry.Text = "";
                passEntry.Text = "";
                emailEntry.Text = "";
                lastnameEntry.Text = "";
                nameEntry.Text = "";
            }

        }

        async void ErrorAllert(string message)
        {
            await DisplayAlert("Klaida", message, "Gerai");

        }

        private User CreateUser(string userName, string name, string lastName, string password,string email, double maxDist = 3)
        {
            User user = new User()
            {
                Name = name,
                LastName = lastName,
                UserName = userName,
                Pass = password,
                Email = email,
                MaxQuestDistance = maxDist
            };

            return user;
        }
    }
}

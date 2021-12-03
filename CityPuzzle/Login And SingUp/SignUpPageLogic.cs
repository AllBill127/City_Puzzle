using CityPuzzle.Classes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CityPuzzle
{
    public class SignUpPageLogic
    {
        public void CreateUser(string userNameEntry, string passEntry, string nameEntry, string lastNameEntry, string emailEntry, string distEntry = "3")
        {
            if (distEntry == "")
                distEntry = "3";

            User tempUser = new User(new UserVerifier());
            User user = new User(new UserVerifier())
            {
                Name = nameEntry,
                LastName = lastNameEntry,
                UserName = userNameEntry,
                Email = emailEntry,
                Pass = tempUser.PassToHash(passEntry),
                MaxQuestDistance = double.Parse(distEntry)
            };

            Sql.SaveUser(user);
        }

        private delegate bool Validator(string text);

        public void Validation(List<Entry> fields)
        {
            Validator validateUsername = new Validator(RegexDelegate.ValidUsername);
            Validator validatePassword = new Validator(RegexDelegate.ValidPassword);
            Validator validateEmail = new Validator(RegexDelegate.ValidEmail);

            foreach (Entry field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Text))
                    throw new EmptyInputdException(field.Placeholder, field);

                // field.Placeholder is defined in SignUpPage.xaml Entry object properties for text input fields
                switch (field.Placeholder)
                {
                    case "Vartotojo vardas":
                        var tempUser = new User(new UserVerifier());
                        if (!validateUsername(field.Text))
                            throw new BadInputdException("Vartotojo vardas turi būti 6 - 12 ženklų ilgio", field.Placeholder, field);
                        else if (!tempUser.CheckUser(field.Text))
                            throw new BadInputdException("Vartotojo vardas užimtas", field);
                        break;

                    case "Slaptazodis":
                        if (!validatePassword(field.Text))
                            throw new BadInputdException("Slaptažodis turi būti 8 - 15 ženklų ilgio ir panaudoti bent vieną didžiają raidę ir skaičių", field);
                        break;

                    case "Pastas":
                        if (!validateEmail(field.Text))
                            throw new BadInputdException("Neteisingas el. pašto adresas", field);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}


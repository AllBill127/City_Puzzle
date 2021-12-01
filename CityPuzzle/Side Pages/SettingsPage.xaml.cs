using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle.Side_Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            userNameField.Text = App.CurrentUser.UserName;
            nameField.Text = App.CurrentUser.Name;
            secondNameField.Text = App.CurrentUser.LastName;
            emailField.Text = App.CurrentUser.Email;
            distanceField.Text =""+App.CurrentUser.MaxQuestDistance;

        }
        void Sign_Out_Click(object sender, EventArgs e)
        {
            var existingPages = Navigation.NavigationStack.ToList();
            var curentPage = existingPages[existingPages.Count - 1];
            var loginPage = existingPages[0];
            foreach (var page in existingPages)
            {

                if (curentPage != page && page!= loginPage)
                {
                    Navigation.RemovePage(page);
                }
            }
            App.CurrentUser = null;
            Sql.SaveCurrentUser(new User("", ""));
            Navigation.PopAsync();
        }
    }
}
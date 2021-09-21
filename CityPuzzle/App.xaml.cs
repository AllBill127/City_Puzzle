using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CityPuzzle.Classes;

namespace CityPuzzle
{
    public partial class App : Application
    {
        public static string FilePath;
        public static User CurrentUser { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
     public App(string filePath)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());

            FilePath = filePath;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

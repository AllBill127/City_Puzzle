using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CityPuzzle.Classes;
using System.IO;

namespace CityPuzzle
{
    public partial class App : Application
    {
        public static string FilePath;
        public static string ObjectPath;
        public static User CurrentUser { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
     public App(string filePath, string objectPath)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());

            FilePath = filePath;
            string fullFileName = Path.GetFullPath(FilePath);
            ObjectPath = objectPath;
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

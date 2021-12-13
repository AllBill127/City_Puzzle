using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CityPuzzle.Classes;
using System.IO;
using CityPuzzle.Rest_Services.Client;

namespace CityPuzzle
{
    public partial class App : Application
    {
        public static string FilePath;
        public static string GamePath;
        public static string ObjectPath;
        public static APICommands WebServices= new APICommands();
        
        public static User CurrentUser { get; set; }

        public App(string filePath, string objectPath, string gamePath)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
            FilePath = filePath;
            GamePath = gamePath;
            ObjectPath = objectPath;
        }
    }
}

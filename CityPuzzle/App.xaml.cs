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
        public static string ObjectPath;                       //API URL                     // DB CONN
        public static APICommands WebServices= new APICommands("http://10.0.2.2:5000/api/", "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LocalCityPuzzleDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        
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

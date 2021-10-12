using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateGamePage : ContentPage
    {
        public Admin newAdmin;
        public static Room newroom;
        public List<Puzzle> defaultpuzzles;
        public List<String> allUsers;
        public static int status=-1;

        public CreateGamePage()
        {
            String roomId = "kambarys" + App.CurrentUser.ID;// Ateityje padaryti random su patikrinimu arba ne;

            newAdmin = new Admin(roomId, App.CurrentUser);

            newroom = new Room(roomId);
            

            InitializeComponent();


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.ObjectPath))
            {
                conn.CreateTable<Puzzle>();
                var obj = conn.Table<Puzzle>().ToList();
        
                defaultpuzzles = obj;
                if(status==-1) AddObj_click(null, null);
            }

            
        }


        public List<String> converter(List<Puzzle> biglist)
        {

            List<String> smallist = new List<String>();
            foreach (Puzzle i in biglist)
            {
                smallist.Add(i.Name);

            }
            return smallist;


        }
        public List<String> converter2(List<User> biglist)
        {

            List<String> smallist = new List<String>();
            foreach (User i in biglist)
            {
                smallist.Add(i.Name);

            }
            return smallist;


        }



        async void AddObj_click(object sender, EventArgs e)
        {
            status = 0;
            await Navigation.PushAsync(new AddPage());
            lookobj.IsVisible = true;
            approved.IsVisible = true;


        }

        async void Look_click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectPuzzles<String>(converter(newroom.Tasks)));
        }

        public static void acction()
        {
            if(status==0)
            editList(SelectPuzzles<String>.getList());

        }
         public static void editList(List<String> given)
        {
            List<Puzzle> newlist = new List<Puzzle>();
            foreach (Puzzle i in newroom.Tasks)
            {
                foreach (String j in given)
                {
                    Console.WriteLine(j);
                    if (i.Name.Equals(j)) newlist.Add(i);
                }
            }
            newroom.Tasks = newlist;

            
        }
        async void Addgamer_click(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SelectPuzzles<String>(allUsers));
            Navigation.PushAsync(new CompliteCreating());

            //Console.WriteLine(newroom.Tasks[0].Name + "------------------------------------ ");

        }








    }
}
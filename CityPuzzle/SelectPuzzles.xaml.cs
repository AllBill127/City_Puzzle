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
    public partial class SelectPuzzles<T> : ContentPage
    {
        public static List<T> defaultlist;
        public static ListView ListView1 = new ListView();
        public static List<T> getList()
        {
            return defaultlist;
        }

        public SelectPuzzles(List<T> given)
        {
            var layout = new StackLayout { Padding = new Thickness(5, 10) };
            defaultlist = given;
            ListView1.ItemsSource = defaultlist;
            ListView1.IsPullToRefreshEnabled = true;
            ListView1.ItemTapped += async (sender, e) => {
                var answer = await DisplayAlert("Demesio", "Ar norite pasalinti " + e.Item, "Taip","Ne") ;
                if (answer)
                {
                    int a=e.ItemIndex;
                    defaultlist.RemoveAt(a);
                    if (defaultlist.Count != 0)
                    {
                        ListView1.ItemsSource = null; ;
                        ListView1.ItemsSource = defaultlist;
                        ListView1.IsRefreshing = false;
                        CreateGamePage.acction();
                        

                    }
                        //else Navigation.PopAsync();
                    }

            };


            layout.Children.Add(ListView1);
            //ListView1.ItemTemplate=
            this.Content = layout;
            
        }



    }

}
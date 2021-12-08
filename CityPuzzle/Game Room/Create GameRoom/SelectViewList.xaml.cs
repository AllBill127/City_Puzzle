using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectViewList<T> : ContentPage
    {
        public static List<T> DefaultList;
        public static ListView ListView1 = new ListView();

        public static List<T> GetList()
        {
            return DefaultList;
        }

        public SelectViewList(List<T> given)
        {

            var layout = new StackLayout { Padding = new Thickness(5, 10) };

            Label label = new Label
            {
                Text = "Redaguokite sarasa",
                FontSize = 28,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            DefaultList = given;
            ListView1.ItemsSource = DefaultList;
            ListView1.IsPullToRefreshEnabled = true;

            ListView1.ItemTapped += async (sender, e) =>
            {
                bool answer = await DisplayAlert("Demesio", "Ar norite pasalinti " + e.Item, "Taip", "Ne");
                if (answer)
                {
                    int a = e.ItemIndex;
                    DefaultList.RemoveAt(a);
                    if (DefaultList.Count != 0)
                    {
                        ListView1.ItemsSource = null; ;
                        ListView1.ItemsSource = DefaultList;
                        ListView1.IsRefreshing = false;
                    }
                }
                
                ((ListView)sender).SelectedItem = null;
            };

            layout.Children.Add(label);
            layout.Children.Add(ListView1);
            this.Content = layout;

        }
    }
}
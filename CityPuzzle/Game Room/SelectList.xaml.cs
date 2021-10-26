using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectList<T> : ContentPage
    {
        public static List<T> DefaultList;
        public static ListView ListView1 = new ListView();
        public static List<T> getList()
        {
            return DefaultList;
        }

        public SelectList(List<T> given)
        {

            var layout = new StackLayout { Padding = new Thickness(5, 10) };
            DefaultList = given;
            ListView1.ItemsSource = DefaultList;
            ListView1.IsPullToRefreshEnabled = true;
            ListView1.ItemTapped += async (sender, e) =>
            {
                var answer = await DisplayAlert("Demesio", "Ar norite pasalinti " + e.Item, "Taip", "Ne");
                if (answer)
                {
                    int a = e.ItemIndex;
                    DefaultList.RemoveAt(a);
                    if (DefaultList.Count != 0)
                    {
                        ListView1.ItemsSource = null; ;
                        ListView1.ItemsSource = DefaultList;
                        ListView1.IsRefreshing = false;
                        CreateGamePage.Acction();

                    }}};

            layout.Children.Add(ListView1);
            //ListView1.ItemTemplate=
            this.Content = layout;
        }}
}
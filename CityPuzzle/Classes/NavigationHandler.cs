using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CityPuzzle.Classes
{
    class NavigationHandler
    {
        public static List<Page> CleanOnePage(List<Page> navigationStack, int place)
        {
            int stackSize = navigationStack.Count;
            navigationStack.Remove(navigationStack[navigationStack.Count - place]);
            return navigationStack;
        }
        public static List<Page> CleanUntilPage(List<Page> NavigationStack, int place)
        {
            NavigationStack.Reverse();
            int stackSize = NavigationStack.Count;
            foreach(Page a in NavigationStack)
            {
                place -= 1;
                NavigationStack.Remove(a);
                if (place == 0) break;
            }
            NavigationStack.Reverse();
            return NavigationStack;
        }
    }
}

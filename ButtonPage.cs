using System;
using Xamarin.Forms;

namespace PocketChef
{
    public class ButtonPage : ContentPage
    {
        public ButtonPage()
        {
            Label label = new Label();
            {
                DisplayAlert("Gesture Info", "I got Clicked!", "OK"); 
            }


        }
    }
}
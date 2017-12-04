using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Generic;

namespace PocketChef
{

    public class DeckModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<Deck.Item> items = new List<Deck.Item>();
        public List<Deck.Item> ItemsList
        {
            get
            {
                return items;
            }
            set
            {
                if (items == value)
                {
                    return;
                }
                items = value;
                OnPropertyChanged();
            }

        }

        protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void SetProp<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public DeckModel()
        {
            items.Add(new Deck.Item() { Name = "Southwest Roast", Food = "one.jpg", Recipe = "Ingredients:\nPork Roast\n"});
            items.Add(new Deck.Item() { Name = "Twin Sister's Shrimp", Food = "two.jpg", Recipe = "" });
            items.Add(new Deck.Item() { Name = "Chicken Fettuccine Alfredo", Food = "three.jpg", Recipe = "" });
            items.Add(new Deck.Item() { Name = "Mac Daddy Mac n' Cheese", Food = "four.jpg", Recipe = "" });
            items.Add(new Deck.Item() { Name = "Caribbean Chicken", Food = "five.jpg", Recipe = "" });
            items.Add(new Deck.Item() { Name = "Pomegranate Sangria", Food = "six.jpg", Recipe = "" });
            items.Add(new Deck.Item() { Name = "Golden Graham Bars", Food = "seven.jpg", Recipe = "" });
            items.Add(new Deck.Item() { Name = "Armadillo Eggs", Food = "eight.jpg", Recipe = "" });
            items.Add(new Deck.Item() { Name = "Loaded Mashed Potatoes", Food = "nine.jpg", Recipe = "" });
            items.Add(new Deck.Item() { Name = "Chipotle Chicken Sliders", Food = "ten.jpg", Recipe = "" });
        }
    }
}

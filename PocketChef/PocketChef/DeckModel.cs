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
            items.Add(new Deck.Item() { Name = "Southwest Roast", Food = "one.jpg", Recipe = "Ingredients: 1 pork roast/n 1 can cream of mushroom /n" });
            items.Add(new Deck.Item() { Name = "Twin Sister's Shrimp", Food = "two.jpg",  Recipe = "Ingredients:" });
            items.Add(new Deck.Item() { Name = "Dish 3", Food = "three.jpg",  Recipe = "Ingredients:" });
        }
    }
}

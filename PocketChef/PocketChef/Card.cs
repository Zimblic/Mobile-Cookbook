using System;
using Xamarin.Forms;

namespace PocketChef
{

    public class Card : ContentView
    {
        public Label Name { get; set; }
        public Image Food { get; set; }
        public Card()
        {
            RelativeLayout view = new RelativeLayout();

            //Backdrop of the card
            BoxView background = new BoxView { Color = Color.FromHex("900C3F"), InputTransparent = true };
            view.Children.Add(background, Constraint.Constant(0), Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            //Name of the Food
            Name = new Label() { TextColor = Color.White, FontSize = 22, InputTransparent = true };
            view.Children.Add(Name, Constraint.Constant(10), Constraint.Constant(10),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.Constant(28));

            //Image of Food
            Food = new Image() { InputTransparent = true, Aspect = Aspect.Fill };
            view.Children.Add(Food, Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { double h = parent.Height * 0.8;
                    return ((parent.Height - h) / 2) + 20; }),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return (parent.Height * 0.8); }));




            Content = view;

        }
    }
}

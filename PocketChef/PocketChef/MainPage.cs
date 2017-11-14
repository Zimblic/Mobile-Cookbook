using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PocketChef
{
    public class MainPage : ContentPage
    {
        Deck stack;
        DeckModel model = new DeckModel();

        public MainPage()
        {
            this.BindingContext = model;
            this.BackgroundColor = Color.Firebrick;

            RelativeLayout view = new RelativeLayout();

            stack = new Deck();
            stack.SetBinding(Deck.ItemsSourceProperty, "ItemsList");
            stack.SwipedLeft += SwipedLeft;
            stack.SwipedRight += SwipedRight;

            view.Children.Add(stack, Constraint.Constant(30), Constraint.Constant(60),
                Constraint.RelativeToParent((parent) => { return parent.Width - 60; }),
                Constraint.RelativeToParent((parent) => { return parent.Height - 140; }));

            this.LayoutChanged += (object sender, EventArgs e) =>
            {
                stack.CardMoveDistance = (int)(this.Width * 0.60f);
            };
            this.Content = view;
        }
        void SwipedLeft(int index)
        {

        }
        void SwipedRight(int index)
        {
        }
    }


}
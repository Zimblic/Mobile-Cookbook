//
//  Copyright (c) 2016 MatchboxMobile
//  Licensed under The MIT License (MIT)
//  http://opensource.org/licenses/MIT
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
//  IN THE SOFTWARE.
//

using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace PocketChef
{

    public class Deck : ContentView
    {

        public class Item
        {
            public string Name { get; set; }
            public string Food { get; set; }
            public string Recipe { get; set; }
        };

        int[] cardnum = Enumerable.Range(0, 20).ToArray(); //Array for card pointers.
        //int[] cardsaved;
        public int c = 0; //Index of cardnum array.
        public int removed = 0; //# of cards removed.
        // back card scale
        const float BackCardScale = 0.8f;
        // speed of the animations
        const int AnimLength = 250;
        // 180 / pi
        const float DegreesToRadians = 57.2957795f;
        // higher the number less the rotation effect
        const float CardRotationAdjuster = 0.8f;
        // distance a card must be moved to consider to be swiped off
        public int CardMoveDistance { get; set; }

        // two cards - top, bottom underneath
        const int NumCards = 2;
        Card[] cards = new Card[NumCards];
        // the card at the top of the stack
        int topCardIndex;
        // distance the card has been moved
        float cardDistance = 0;
        // the last items index added to the stack of the cards
        int itemIndex = 0;
        bool ignoreTouch = false;

        // called when a card is swiped left/right with the card index in the ItemSource
        public Action<int> SwipedRight = null;
        public Action<int> SwipedLeft = null;

        public static readonly BindableProperty ItemsSourceProperty =
                BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(Deck), null,
                propertyChanged: OnItemsSourcePropertyChanged);

        public List<Item> ItemsSource
        {
            get
            {
                return (List<Item>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
                itemIndex = 0;
            }
        }

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((Deck)bindable).Setup();
        }

        public Deck()
        {
            RelativeLayout view = new RelativeLayout();
            // create a stack of cards
            for (int i = 0; i < NumCards; i++)
            {
                var card = new Card();
                cards[i] = card;
                card.InputTransparent = true;
                card.IsVisible = true;

                view.Children.Add(
                    card,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height;
                    })
                );
            }

            this.BackgroundColor = Color.FromHex("C6B2C6");
            this.Content = view;

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);

        }

        void Setup() //Creates first 2 cards
        {
           
            // set the top card
            topCardIndex = 0;
            // create a stack of cards

            for (int i = 0; i < Math.Min(NumCards, ItemsSource.Count); i++)
            {

                itemIndex = cardnum[c];
                
                if (itemIndex >= ItemsSource.Count) break;
                
                var card = cards[i];
                card.Name.Text = ItemsSource[itemIndex].Name;
                card.Food.Source = ImageSource.FromFile(ItemsSource[itemIndex].Food);
                card.IsVisible = true;
                card.Scale = GetScale(i);
                card.RotateTo(0, 0);
                card.TranslateTo(0, -card.Y, 0);
                ((RelativeLayout)this.Content).LowerChild(card);
                c++;
            }
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    HandleTouchStart();
                    break;
                case GestureStatus.Running:
                    HandleTouch((float)e.TotalX);
                    break;
                case GestureStatus.Completed:
                    HandleTouchEnd();
                    break;
            }
        }

        // to hande when a touch event begins
        public void HandleTouchStart()
        {
            cardDistance = 0;
        }

        // to handle the ongoing touch event as the card is moved
        public void HandleTouch(float diff_x)
        {
            if (ignoreTouch)
            {
                return;
            }

            var topCard = cards[topCardIndex];
            var backCard = cards[PrevCardIndex(topCardIndex)];

            // move the top card
            if (topCard.IsVisible)
            {

                // move the card
                topCard.TranslationX = (diff_x);

                // calculate a angle for the card
                float rotationAngel = (float)(CardRotationAdjuster * Math.Min(diff_x / this.Width, 1.0f));
                topCard.Rotation = rotationAngel * DegreesToRadians;

                // keep a record of how far its moved
                cardDistance = diff_x;
            }

            // scale the backcard
            if (backCard.IsVisible)
            {
                backCard.Scale = Math.Min(BackCardScale + Math.Abs((cardDistance / CardMoveDistance) * (1.0f - BackCardScale)), 1.0f);
            }
        }

        // to handle the end of the touch event
        async public void HandleTouchEnd()
        {
            ignoreTouch = true;

            var topCard = cards[topCardIndex];

            // if the card was move enough to be considered swiped off
            if (Math.Abs((int)cardDistance) > CardMoveDistance)
            {

                // move off the screen
                await topCard.TranslateTo(cardDistance > 0 ? this.Width : -this.Width, 0, AnimLength / 2, Easing.SpringOut);
                topCard.IsVisible = false;

                if (SwipedRight != null && cardDistance > 0)
                {
                    SwipedRight(itemIndex);
                    //cardsaved[0] = itemIndex;
                }
                else if (SwipedLeft != null)
                {
                    SwipedLeft(itemIndex);
                    cardnum = cardnum.Where((val) => val != itemIndex-1).ToArray(); //Removes swiped card.
                    removed++; //# of cards removed.
                }

                // show the next card
                ShowNextCard();

            }
            // put the card back in the center
            else
            {

                // move the top card back to the center
                await topCard.TranslateTo((-topCard.X), -topCard.Y, AnimLength, Easing.SpringOut);
                await topCard.RotateTo(0, AnimLength, Easing.SpringOut);

                // scale the back card down
                var prevCard = cards[PrevCardIndex(topCardIndex)];
                await prevCard.ScaleTo(BackCardScale, AnimLength, Easing.SpringOut);

            }

            ignoreTouch = false;
        }

        // show the next two cards
        void ShowNextCard() //Loads the rest of the card deck.
        {
            if (cards[0].IsVisible == false && cards[1].IsVisible == false) //1st 2 cards gone, load more
            {
                Setup();
                return;
            }

            var topCard = cards[topCardIndex];
            topCardIndex = NextCardIndex(topCardIndex);
            itemIndex = cardnum[c]; //INITIATES NEXT SET DO NOT REMOVE
            // if there are more cards to show, show the next card in the place of 
            // the card that was swiped off the screen
            if (itemIndex < ItemsSource.Count || c < (ItemsSource.Count - removed))
            {
                itemIndex = cardnum[c]; //Sets card to what's left of array.
                // push it to the back z order
                ((RelativeLayout)this.Content).LowerChild(topCard);

                // reset its scale, opacity and rotation
                topCard.Scale = BackCardScale;
                topCard.RotateTo(0, 0);
                topCard.TranslateTo(0, -topCard.Y, 0);

                // set the data
                topCard.Name.Text = ItemsSource[itemIndex].Name;
                topCard.Food.Source = ImageSource.FromFile(ItemsSource[itemIndex].Food);

                topCard.IsVisible = true;
                c++;
            }
            else if (itemIndex >= ItemsSource.Count || c >= (ItemsSource.Count - removed))
            {
                c = 0; //Restart the array with remaining cards - Forever loop
            }
        }

        // return the next card index from the top
        int NextCardIndex(int topIndex)
        {
            return topIndex == 0 ? 1 : 0;
        }

        // return the prev card index from the top
        int PrevCardIndex(int topIndex)
        {
            return topIndex == 0 ? 1 : 0;
        }

        // helper to get the scale based on the card index position relative to the top card
        float GetScale(int index)
        {
            return (index == topCardIndex) ? 1.0f : BackCardScale;
        }

    }
    static class MyExtensions
    {
        private static Random rand = new Random();
        public static void Shuffle<T>(this IList<T> list) //FisherYates Shuffler
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
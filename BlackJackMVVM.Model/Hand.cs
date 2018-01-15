﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackMVVM.Model
{
    public class Hand
    {
        #region Attributes

        public int handValue = 0;

        public List<Card> hand;

        /// <summary>
        /// Container to hold the deck generated by the GameManager class
        /// </summary>
        public Deck gameDeck;

        #endregion


        /// <summary>
        /// Default Constructor for hand
        /// </summary>
        public Hand(Deck gameDeck)
        {
            this.gameDeck = gameDeck;
            hand = new List<Card>();
        }

        /// <summary>
        /// Gets the current value of the hand
        /// </summary>
        /// <returns>Value of current hand</returns>
        public int getHandValue() {
            handValue = 0;

            foreach (var card in hand)
            {
                //All face cards are worth 10
                if (card.rank > 10)
                {
                    handValue += 10;
                }
                //Deal with Aces
                else if (card.rank == 1)
                {
                    handValue += 11;

                    if (handValue > 21)
                    {
                        handValue -= 11;
                        handValue += 1;
                    }
                }
                else
                {
                    handValue += card.rank;
                }  
            }

            return handValue;
        }
    }
}
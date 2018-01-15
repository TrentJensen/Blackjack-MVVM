using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackMVVM.Model
{
    public class Card
    {
        public int rank { get; set; }
        public string suit { get; set; }

        public string imageName { get; set; }

        /// <summary>
        /// Constructor for a new card
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="suit"></param>
        public Card(int rank, string suit)
        {
            this.rank = rank;
            this.suit = suit;
            this.imageName = rank + "_of_" + suit;
        }
    }
}

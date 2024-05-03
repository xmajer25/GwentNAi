namespace GwentNAi.GameSource.Board
{
    /*
     * Class for holding information about swaping cards
     * Automaticly clearing possible swaps with modified setters
     * 
     */
    public class SwapCards : ICloneable
    {
        /*
         * List of possible cards to swap
         */
        public List<int> Indexes = new List<int>(10);

        /*
         * The ammount of players that have not finished swapping cards
         */
        private int _playersToSwap = 2;
        public int PlayersToSwap
        {
            get { return _playersToSwap; }
            set
            {
                if (_playersToSwap != value)
                    _playersToSwap = value;
            }
        }

        /*
         * 3 possible card swaps
         * when there are no card swaps left,
         * automaticly clear indexes, decrease the amount of player to swap, reset it self
         */
        private int _cardSwaps = 3;
        public int CardSwaps
        {
            get { return _cardSwaps; }
            set
            {
                _cardSwaps = value;
                if (_cardSwaps == 0)
                {
                    Indexes.Clear();
                    PlayersToSwap--;
                    _cardSwaps = 3;
                }
            }
        }

        /*
         * IS NEVER SET TO TRUE (check set method)
         * when set to true:
         *  automaticly clear indexes, decrease the amount of players to swap, 
         */
        private bool _stopSwapping = false;
        public bool StopSwapping
        {
            get { return _stopSwapping; }
            set
            {
                if (_stopSwapping != value)
                {
                    Indexes.Clear();
                    CardSwaps = 3;
                    PlayersToSwap--;
                }
            }
        }

        /*
         * Returns true if there is at least one card to swap
         */
        public bool SwapAvailable => (Indexes.Count > 0);


        /*
         * Creates a deep clone of this object
         */
        public object Clone()
        {
            SwapCards clonedSwapCards = new SwapCards()
            {
                Indexes = new List<int>(Indexes),
                PlayersToSwap = PlayersToSwap,
                CardSwaps = CardSwaps,
                StopSwapping = StopSwapping,
            };

            return clonedSwapCards;
        }
    }
}

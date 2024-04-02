namespace GwentNAi.GameSource.Board
{
    public class SwapCards : ICloneable
    {
        public List<int> Indexes = new List<int>(10);
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
        public bool SwapAvailable => (Indexes.Count > 0);

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

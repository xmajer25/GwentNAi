using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Board
{
    public class SwapCards
    {
        public List<int> Indexes = new List<int>(10);

        private int _cardSwaps = 3;
        public int CardSwaps
        {
            get { return _cardSwaps; }
            set 
            {
                _cardSwaps = value;
                if(_cardSwaps == 0)
                {
                    Indexes.Clear();
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
                if(_stopSwapping != value)
                {
                    Indexes.Clear();
                }
            }
        }
        public bool SwapAvailable => (Indexes.Count > 0);
    }
}

using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class OldSpeartipAsleep : DefaultCard, ITimer
    {
        private int timer = 3;
        
        /*
         * Initialize information about specific card 
         */
        public OldSpeartipAsleep()
        {
            CurrentValue = 6;
            MaxValue = 6;
            Shield = 2;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "OdlSpeartip:Asleep";
            ShortName = "Speart:A";
            Descriptors = new List<string>() { "Ogroid" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * overriden method for taking damage
         * when unit takes damage -> transforms into old speartip
         */
        public override void TakeDemage(int damage, bool lethal, GameBoard board)
        {
            if(lethal)
            {
                CurrentValue = 0;
                return; 
            }

            int _excessDamage = Shield - damage;
            Shield -= damage;
            if (Shield < 0) Shield = 0;
            if(_excessDamage < 0)
            {
                CurrentValue += _excessDamage; // _excessDamage is a negative number
            }
            

            Transform(board, false);
        }

        /*
         * Timer update with each turn
         * when 0 transform
         */
        public void Timer(GameBoard board)
        {
            if (timer == 0) Transform(board, true);
            else timer--;
        }

        /*
         * Transforms into onldSpeartip if that card is in deck
         */
        private void Transform(GameBoard board, bool byTimer)
        {
            for (int i = 0; i < board.Leader1.Board.Count; i++)
            {
                for (int j = 0; j < board.Leader1.Board[i].Count; j++)
                {
                    if (board.Leader1.Board[i][j] == this)
                    {
                        OldSpeartip transformedCard = GetOldSpeartip(board.Leader2.StartingDeck.Cards);
                        if (transformedCard == null) return;

                        board.Leader1.Board[i][j] = transformedCard;
                        if (byTimer)
                        {
                            board.Leader1.Board[i][j].CurrentValue += 6;
                            board.Leader1.Board[i][j].MaxValue += 6;
                        }
                        return;
                    }
                }
            }
            for (int i = 0; i < board.Leader2.Board.Count; i++)
            {
                for (int j = 0; j < board.Leader2.Board[i].Count; j++)
                {
                    if (board.Leader2.Board[i][j] == this)
                    {
                        OldSpeartip transformedCard = GetOldSpeartip(board.Leader2.StartingDeck.Cards);
                        if (transformedCard == null) return;

                        board.Leader2.Board[i][j] = transformedCard;
                        if (byTimer)
                        {
                            board.Leader2.Board[i][j].CurrentValue += 6;
                            board.Leader2.Board[i][j].MaxValue += 6;
                        }
                        return;
                    }
                }
            }
        }

        /*
         * Returns Old Speartip if in Deck
         */
        private OldSpeartip GetOldSpeartip(List<DefaultCard> Deck)
        {
            foreach (var card in Deck)
            {
                if (card.GetType() == typeof(OldSpeartip))
                {
                    Deck.Remove(card);
                    return new OldSpeartip();
                }
            }
            return null;
        }
    }
}

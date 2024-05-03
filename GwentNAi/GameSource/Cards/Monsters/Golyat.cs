using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Player;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Golyat : DefaultCard, IDeathwish
    {
        /*
         * Initialize information about specific card 
         */
        public Golyat()
        {
            CurrentValue = 12;
            MaxValue = 12;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Golyat";
            ShortName = "Golyat";
            Descriptors = new List<string>() { "Ogroid" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * Triggers when card is destroyed
         * Plays the highest value unit on the enemie board
         */
        public void DeathwishAbility(GameBoard board)
        {
            int row = GetRow(board);
            DefaultLeader enemieLeader = board.GetEnemieLeader();
            int enemieRow = (row == 0 ? 1 : 0);
            DefaultCard highestPowerUnit = new();

            for (int cardIndex = 0; cardIndex < enemieLeader.StartingDeck.Cards.Count; cardIndex++)
            {
                DefaultCard card = enemieLeader.StartingDeck.Cards[cardIndex];
                if (highestPowerUnit.CurrentValue < card.CurrentValue)
                {
                    highestPowerUnit = card;
                }
            }

            if (enemieLeader.Board[enemieRow].Count <= 9)
            {
                enemieLeader.StartingDeck.Cards.Remove(highestPowerUnit);
                enemieLeader.Board[enemieRow].Add(highestPowerUnit);
            }

        }

        /*
         * Returns row number of this card
         */
        private int GetRow(GameBoard board)
        {
            int foundRow = 0;
            foreach (var row in board.Leader1.Board)
            {
                foreach (var card in row)
                {
                    if (card == this)
                    {
                        return foundRow;
                    }
                }
                foundRow++;
            }

            foundRow = 0;
            foreach (var row in board.Leader2.Board)
            {
                foreach (var card in row)
                {
                    if (card == this)
                    {
                        return foundRow;
                    }
                }
                foundRow++;
            }
            return -1;
        }
    }
}

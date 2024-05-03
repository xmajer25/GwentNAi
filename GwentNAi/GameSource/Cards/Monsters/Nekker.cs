using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Nekker : DefaultCard, IDeploy, IThrive
    {
        /*
         * Initialize information about specific card 
         */
        public Nekker()
        {
            CurrentValue = 1;
            MaxValue = 1;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Nekker";
            ShortName = "Nekker";
            Descriptors = new List<string>() { "Ogroid" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * Summon a copy of this card on the board
         */
        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> currentBoard = board.GetCurrentBoard();
            int thisIndex = currentBoard[0].IndexOf((DefaultCard)this);
            int thisRow = 0;
            if (thisIndex == -1)
            {
                thisIndex = currentBoard[1].IndexOf((DefaultCard)this);
                thisRow = 1;
            }

            if (currentBoard[thisRow].Count != 9)
            {
                currentBoard[thisRow].Insert(thisIndex + 1, new Nekker());
            }
        }


        /*
         * When a card with higher value is played on current board->
         * increment this card's value
         */
        public void Thrive(int playedUnitValue)
        {
            if (playedUnitValue > CurrentValue)
            {
                CurrentValue++;
                if (CurrentValue > MaxValue) MaxValue++;

            }
        }
    }
}

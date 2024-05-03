using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Brewess : DefaultCard, ICroneInteraction, IOrder, IOrderExpandPickAlly, ICharge
    {
        public int charge = 1;

        /*
         * Initialize information about specific card 
         */
        public Brewess()
        {
            CurrentValue = 6;
            MaxValue = 6;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Brewess";
            ShortName = "Brewess";
            Descriptors = new List<string>() { "Relict", "Crone" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * If charged, fills imidiate actions with possible targets
         * (all the cards on current board except this)
         */
        public void Order(GameBoard board)
        {
            if (charge <= 0) return;
            List<List<DefaultCard>> currentBoard = board.GetCurrentBoard();

            for (int row = 0; row < currentBoard.Count; row++)
            {
                for (int currentIndex = 0; currentIndex < currentBoard[row].Count; currentIndex++)
                {
                    if (currentBoard[row][currentIndex] != this)
                        board.CurrentPlayerActions.ImidiateActions[0][row].Add(currentIndex);
                }
            }
        }

        /*
         * Executes order
         * Consumes a cards targeted by row-index
         */
        public void PostPickAllyOrder(GameBoard board, int row, int index)
        {
            DefaultCard consumedCard = board.GetCurrentBoard()[row][index];
            CurrentValue += consumedCard.CurrentValue;
            MaxValue += consumedCard.CurrentValue;
            consumedCard.TakeDemage(consumedCard.CurrentValue, true, board);
            charge--;
            board.GetCurrentLeader().UseAbility();
        }

        /*
         * When a Crone card is played -> charges are incremented
         */
        public void RespondToCrone()
        {
            charge++;
        }
    }
}

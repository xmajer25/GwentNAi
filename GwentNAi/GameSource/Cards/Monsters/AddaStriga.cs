using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IExpand;
using GwentNAi.GameSource.Player;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class AddaStriga : DefaultCard, IOrder, IOrderExpandPickAll
    {
        /*
         * Initialize information about specific card 
         */
        public AddaStriga()
        {
            CurrentValue = 6;
            MaxValue = 6;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Adda:Striga";
            ShortName = "Adda";
            Descriptors = new List<string>() { "Beast", "Cursed" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * Implementing IOrder
         */
        public void Order(GameBoard board)
        {
            PickAll(board);
        }

        /*
         * Fills imidiate actions with all of the possible targets for ability
         * (From both boards, all cards with less health and 'Token' descriptor)
         */
        public void PickAll(GameBoard board)
        {
            int currentRow = 0;

            foreach (var row in board.Leader1.Board)
            {
                for (int currentIndex = 0; currentIndex < row.Count; currentIndex++)
                {
                    var card = row[currentIndex];
                    if (card.CurrentValue < this.CurrentValue && card.Descriptors.Contains("Token"))
                    {
                        board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentIndex);
                    }
                }
                currentRow++;
            }

            currentRow = 0;

            foreach (var row in board.Leader2.Board)
            {
                for (int currentIndex = 0; currentIndex < row.Count; currentIndex++)
                {
                    var card = row[currentIndex];
                    if (card.CurrentValue < this.CurrentValue && card.Descriptors.Contains("Token"))
                    {
                        board.CurrentPlayerActions.ImidiateActions[1][currentRow].Add(currentIndex);
                    }
                }
                currentRow++;
            }
        }

        /*
         * Executes order
         * Consumes a unit targeted by row-index
         */
        void IOrderExpandPickAll.PostPickAllOrder(GameBoard board, int player, int row, int index)
        {
            int multiplier = 1;

            List<List<DefaultCard>> targetedBoard = (player == 0 ? board.Leader1.Board : board.Leader2.Board);
            DefaultLeader targetedLeader = (player == 0 ? board.Leader1 : board.Leader2);
            if (targetedLeader == board.GetCurrentLeader()) multiplier = 2;
            DefaultCard consumedCard = targetedBoard[row][index];

            CurrentValue += consumedCard.CurrentValue * multiplier;
            MaxValue += consumedCard.CurrentValue * multiplier;
            consumedCard.TakeDemage(consumedCard.CurrentValue, true, board);
            TimeToOrder--;
            board.GetCurrentLeader().UseAbility();
        }
    }
}

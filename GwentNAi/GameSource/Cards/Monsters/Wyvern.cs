using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using System.Numerics;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Wyvern : DefaultCard, IDeploy, IDeployExpandPickEnemies, IThrive
    {
        /*
         * Initialize information about specific card 
         */
        public Wyvern()
        {
            CurrentValue = 3;
            MaxValue = 3;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Wyvern";
            ShortName = "Wyvern";
            Descriptors = new List<string>() { "Beast" };
            TimeToOrder = -1;
            Bleeding = 0;
        }

        /*
         * Fills imidiate actions with targets for deploy abilty
         * (all the cards on enemie board)
         */
        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();

            for (int currentRow = 0; currentRow < enemieBoard.Count; currentRow++)
            {
                for (int currentIndex = 0; currentIndex < enemieBoard[currentRow].Count; currentIndex++)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentIndex);
                }
            }
        }

        /*
         * Executes deploy ability
         * Deals 2 damage to an enemie card
         */
        public void postPickEnemieAbilitiy(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            enemieBoard[row][index].TakeDemage(2, false, board);
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

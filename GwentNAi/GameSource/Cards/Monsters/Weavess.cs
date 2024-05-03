using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Weavess : DefaultCard, IDeploy, IDeployExpandPickAlly, ICroneInteraction
    {
        int boost = 2;

        /*
         * Initialize information about specific card 
         */
        public Weavess()
        {
            CurrentValue = 6;
            MaxValue = 6;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Weavess";
            ShortName = "Weavess";
            Descriptors = new List<string>() { "Relict", "Crone" };
            TimeToOrder = -1;
            Bleeding = 0;
        }

        /*
         * Fills imidiate actions with deploy targets
         * (All allied cards)
         */
        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> allyBoard = board.GetCurrentBoard();

            for (int row = 0; row < allyBoard.Count; row++)
            {
                for (int currentIndex = 0; currentIndex < allyBoard[row].Count; currentIndex++)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][row].Add(currentIndex);
                }

                //Remove position of this card
                int WeavessPosition = allyBoard[row].IndexOf(this);
                if (WeavessPosition != -1) board.CurrentPlayerActions.ImidiateActions[0][row].RemoveAt(WeavessPosition);
            }
        }

        /*
         * Executes deploy ability
         * (boosts allied unit by 'boost' value)
         */
        public void PostPickAllyAbilitiy(GameBoard board, int row, int index)
        {
            DefaultCard boostedCard = board.GetCurrentBoard()[row][index];
            boostedCard.CurrentValue += boost;
            boostedCard.MaxValue += boost;
        }

        /*
         * Triggered by a crone card played on the board
         * -> increments the boost value by 2
         */
        public void RespondToCrone()
        {
            boost += 2;
        }
    }
}

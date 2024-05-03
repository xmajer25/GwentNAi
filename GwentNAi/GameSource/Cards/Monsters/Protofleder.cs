using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Protofleder : DefaultCard, IDeploy, IDeployExpandPickEnemies
    {
        /*
         * Initialize information about specific card 
         */
        public Protofleder()
        {
            CurrentValue = 4;
            MaxValue = 4;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Protofleder";
            ShortName = "Protofl.";
            Descriptors = new List<string>() { "Vampire" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * Fill imidiate actions with possible targets for deploy abiltiy
         * (all the cards played on enemie board)
         */
        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();

            for (int i = 0; i < enemieBoard.Count; i++)
            {
                for (int cardIndex = 0; cardIndex < enemieBoard[i].Count; cardIndex++)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][i].Add(cardIndex);
                }
            }
        }

        /*
         * Execute deploy ability
         * Bleed enemye card for 3
         */
        public void postPickEnemieAbilitiy(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            enemieBoard[row][index].Bleeding += 3;

            CurrentValue += enemieBoard[row][index].Bleeding;
            MaxValue = MaxValue - CurrentValue + enemieBoard[row][index].Bleeding;
        }
    }
}

using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Whispess : DefaultCard, IDeploy, IDeployExpandPickEnemies, ICroneInteraction
    {
        private int abilityDemage { get; set; } = 2;

        /*
         * Initialize information about specific card 
         */
        public Whispess()
        {
            CurrentValue = 6;
            MaxValue = 6;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Whispess";
            ShortName = "Whispess";
            Descriptors = new List<string>() { "Relict", "Crone" };
            TimeToOrder = -1;
            Bleeding = 0;
        }

        /*
         * Fills imidiate actions with deploy targets
         * (All enemie cards)
         */
        public void Deploy(GameBoard board)
        {
            List<List<int>> enemieIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            int currentRow = 0;
            int currentIndex = 0;

            foreach (var row in enemieBoard)
            {
                foreach (var card in row)
                {
                    enemieIndexes[currentRow].Add(currentIndex);
                    currentIndex++;
                }
                currentIndex = 0;
                currentRow++;
            }

            board.CurrentPlayerActions.ImidiateActions[0] = enemieIndexes;
        }

        /*
         * Executes deploy abilty
         * (Deals 'abilityDamage' to an enemie card)
         */
        public void postPickEnemieAbilitiy(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            enemieBoard[row][index].TakeDemage(abilityDemage, false, board);
        }

        /*
         * Triggered by a crone card played
         * (increments damage for deploy ability by 2)
         */
        public void RespondToCrone()
        {
            abilityDemage += 2;
        }
    }
}

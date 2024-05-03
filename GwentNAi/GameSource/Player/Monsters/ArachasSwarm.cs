using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IExpand;
using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Player.Monsters
{
    /*
     * Child class of DefautlLeader for defining new leader ability
     */
    public class ArachasSwarm : DefaultLeader, IPlayCardExpand
    {
        /*
         * Initialize information about specific leader ability
         */
        public ArachasSwarm()
        {
            ProvisionValue = 15;
            LeaderName = "ArachasSwarm";
            LeaderFaction = "monsters";
            AbilityCharges = 5;
            HasPassed = false;
        }

        /*
         * Call order if ability can be used
         */
        public override void Order(GameBoard board)
        {
            if (AbilityCharges == 0) return;
            PlayCardExpand(board);
        }
        
        /*
         * Creates a deep clone of this object
         */
        public override object Clone()
        {
            DefaultLeader clonedLeader = new ArachasSwarm()
            {
                Iterations = Iterations,
                Simulations = Simulations,
                ProvisionValue = ProvisionValue,
                LeaderName = LeaderName,
                LeaderFaction = LeaderFaction,
                IsStarting = IsStarting,
                Victories = Victories,
                AbilityCharges = AbilityCharges,
                PlayerMethod = PlayerMethod,
                HasPassed = HasPassed,
                HasPlayedCard = HasPlayedCard,
                HasUsedAbility = HasUsedAbility,
                StartingDeck = (DefaultDeck)StartingDeck.Copy(),
                Hand = (DefaultDeck)Hand.Copy(),
                Graveyard = (DefaultDeck)Graveyard.Copy(),
                Board = Board
                .Select(innerList => innerList
                    .Select(card => (DefaultCard)card.Clone())  // Deep clone of each DefaultCard
                    .ToList())
                .ToList()
            };

            return clonedLeader;
        }

        /*
         * Fills imidiate actions with leader ability targets
         * (possible card placements on our board)
         */
        public void PlayCardExpand(GameBoard board)
        {
            List<List<DefaultCard>> CPboard = board.GetCurrentBoard();
            List<List<int>> possibleIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            int currentRow = 0;
            int currentCulumn = 0;

            foreach (var row in CPboard)
            {
                foreach (var card in row)
                {
                    possibleIndexes[currentRow].Add(currentCulumn);
                    currentCulumn++;
                }
                possibleIndexes[currentRow].Add(currentCulumn);
                if (possibleIndexes[currentRow].Count == 10) possibleIndexes[currentRow].Clear();
                currentRow++;
                currentCulumn = 0;
            }

            board.CurrentPlayerActions.ImidiateActions[0] = possibleIndexes;
        }

        /*
         * Executes leader ability
         * (summons a Drone on row-index)
         */
        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            DefaultCard playedCard = new Drone();
            board.GetCurrentBoard()[row].Insert(column, playedCard);
            PostAbilitySettings();
        }
    }
}

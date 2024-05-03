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
    public class ForceOfNature : DefaultLeader, IPlayCardExpand
    {
        /*
         * Initialize information about specific leader ability
         */
        public ForceOfNature()
        {
            ProvisionValue = 15;
            LeaderName = "ForceOfNature";
            LeaderFaction = "monsters";
            AbilityCharges = 1;
            HasPassed = false;
        }

        /*
         * Creates a deep clone of this object
         */
        public override object Clone()
        {
            DefaultLeader clonedLeader = new ForceOfNature()
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
         * Executes order if charged
         */
        public override void Order(GameBoard board)
        {
            if (AbilityCharges == 0) return;
            PlayCardExpand(board);
        }


        /*
         * Fills imidiate actions with ability targets
         * (all card placements on our board)
         */
        public void PlayCardExpand(GameBoard board)
        {
            int currentRow = 0;
            int currentCulumn = 0;

            foreach (var row in Board)
            {
                foreach (var card in row)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentCulumn);
                    currentCulumn++;
                }
                board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentCulumn);
                if (board.CurrentPlayerActions.ImidiateActions[0][currentRow].Count == 10) board.CurrentPlayerActions.ImidiateActions[0][currentRow].Clear();
                currentRow++;
                currentCulumn = 0;
            }
        }

        /*
         * Executes leader ability
         * (summons WoodlandSpirit)
         */
        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            DefaultCard playedCard = new WoodlandSpirit();
            PlayCardByAbility(playedCard, row, column, board);
            PostAbilitySettings();
        }
    }
}

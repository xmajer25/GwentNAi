using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Player.Monsters
{
    /*
     * Child class of DefautlLeader for defining new leader ability
     */
    public class BloodScent : DefaultLeader
    {
        /*
         * Initialize information about specific leader ability
         */
        public BloodScent()
        {
            ProvisionValue = 15;
            LeaderName = "BloodScent";
            LeaderFaction = "monsters";
            AbilityCharges = 3;
            HasPassed = false;
        }

        /*
         * Creates a deep clone of this object
         */
        public override object Clone()
        {
            DefaultLeader clonedLeader = new BloodScent()
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
            PostAbilitySettings();
        }
    }
}

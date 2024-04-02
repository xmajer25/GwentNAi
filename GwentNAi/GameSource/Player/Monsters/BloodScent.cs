using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Player.Monsters
{
    public class BloodScent : DefaultLeader
    {
        public BloodScent()
        {
            ProvisionValue = 15;
            LeaderName = "BloodScent";
            LeaderFaction = "monsters";
            AbilityCharges = 3;
            HasPassed = false;
        }

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
        public override void Order(GameBoard board)
        {
            if (AbilityCharges == 0) return;
            PostAbilitySettings();
        }
    }
}

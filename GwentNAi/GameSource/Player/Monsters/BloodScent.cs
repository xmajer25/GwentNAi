using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                StartingDeck = (Deck)StartingDeck.Copy(),
                HandDeck = (Deck)HandDeck.Copy(),
                GraveyardDeck = (Deck)GraveyardDeck.Copy(),
                Board = Board.Select(innerList => innerList.Select(card => (DefaultCard)card.Clone()).ToList()).ToList(),
            };

            return clonedLeader;
        }
        public override void Order(GameBoard board)
        {
            if (AbilityCharges == 0) return;
            AbilityCharges--;
            Console.WriteLine("Current charges: " + AbilityCharges);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}

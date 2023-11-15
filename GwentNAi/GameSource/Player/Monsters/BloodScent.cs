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
            provisionValue = 15;
            leaderName = "BloodScent";
            leaderFaction = "monsters";
            abilityCharges = 3;
            hasPassed = false;
        }

        public override object Clone()
        {
            DefaultLeader clonedLeader = new BloodScent()
            {
                provisionValue = provisionValue,
                leaderName = leaderName,
                leaderFaction = leaderFaction,
                isStarting = isStarting,
                victories = victories,
                abilityCharges = abilityCharges,
                playerMethod = playerMethod,
                hasPassed = hasPassed,
                hasPlayedCard = hasPlayedCard,
                hasUsedAbility = hasUsedAbility,
                startingDeck = (Deck)startingDeck.Copy(),
                handDeck = (Deck)handDeck.Copy(),
                graveyardDeck = (Deck)graveyardDeck.Copy(),
                Board = Board.Select(innerList => innerList.Select(card => (DefaultCard)card.Clone()).ToList()).ToList(),
            };

            return clonedLeader;
        }
        public override void Order(GameBoard board)
        {
            if (abilityCharges == 0) return;
            abilityCharges--;
            Console.WriteLine("Current charges: " + abilityCharges);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}

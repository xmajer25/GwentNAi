using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.CardRepository
{
    public class MonsterCards : ICloneable
    {
        private readonly List<DefaultCard> Cards = new()
        {
            new AddaStriga(), new Brewess(), new Ghoul(), new Golyat(), new Griffin(), new IceGiant(),
            new Katakan(), new Nekker(), new NekkerWarrior(), new OldSpeartip(), new OldSpeartipAsleep(), new Ozzrel(), 
            new Protofleder(), new Weavess(), new Whispess(), new WildHuntHound(), new WildHuntRider(), new Wyvern()
        };

        private readonly Dictionary<string, int> CardCount = new Dictionary<string, int>();

        public MonsterCards()
        {
            CardCount = new Dictionary<string, int>();
        }
        public object Clone()
        {
            var clonedMonsterCards = new MonsterCards();

            clonedMonsterCards.Cards.AddRange(Cards);
            foreach (var entry in CardCount)
                clonedMonsterCards.CardCount.Add(entry.Key, entry.Value);

            return clonedMonsterCards;
        }
        public DefaultCard GetRandomCard()
        {
            Random random = new Random();
            int randomIndex;

            do
            {
                randomIndex = random.Next(Cards.Count);
            } while (!IsCardAllowed(Cards[randomIndex]));

            UpdateCardCount(Cards[randomIndex]);
            return Cards[randomIndex];
        }

        private bool IsCardAllowed(DefaultCard card)
        {
            int count;
            CardCount.TryGetValue(card.name, out count);

            if (card.border == 1 && count >= 1)
            {
                return false;
            }

            if (card.border == 0 && count >= 2)
            {
                return false;
            }

            return true;
        }

        private void UpdateCardCount(DefaultCard card)
        {
            if (CardCount.ContainsKey(card.name))
            {
                CardCount[card.name]++;
            }
            else
            {
                CardCount.Add(card.name, 1);
            }
        }
    }
}

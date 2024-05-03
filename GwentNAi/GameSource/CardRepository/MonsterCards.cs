using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.Monsters;

namespace GwentNAi.GameSource.CardRepository
{
    /*
     * Class holding information about cards played by players
     * Has a list of all the possible cards held by the player,
     * and a dictionary for counting the ammount of times each card has been played
     */
    public class MonsterCards : ICloneable
    {
        /*
         * All of the monster cards implemented
         */
        private readonly List<DefaultCard> Cards = new()
        {
            new AddaStriga(), new Brewess(), new Ghoul(), new Golyat(), new Griffin(), new IceGiant(),
            new Katakan(), new Nekker(), new NekkerWarrior(), new OldSpeartip(), new OldSpeartipAsleep(), new Ozzrel(),
            new Protofleder(), new Weavess(), new Whispess(), new WildHuntHound(), new WildHuntRider(), new Wyvern()
        };

        /*
         * Dictionary used for counting cards for MCTS random enemie play
         */
        private readonly Dictionary<string, int> CardCount = new Dictionary<string, int>();

        /*
         * Initialize new MonsterCards object with new Dictionary
         */
        public MonsterCards()
        {
            CardCount = new Dictionary<string, int>();
        }

        /*
         * Creates a deep clone of this object
         */
        public object Clone()
        {
            var clonedMonsterCards = new MonsterCards();

            clonedMonsterCards.Cards.AddRange(Cards);
            foreach (var entry in CardCount)
                clonedMonsterCards.CardCount.Add(entry.Key, entry.Value);

            return clonedMonsterCards;
        }

        /*
         * Returns a random card from all the possible monster cards
         */
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

        /*
         * Takes a card and checks if it has been used
         * (one time for golden border, twice for brown border)
         * returns true if card can be used
         * Used for getting random cards for enemie in MCTS
         */
        private bool IsCardAllowed(DefaultCard card)
        {
            int count;
            CardCount.TryGetValue(card.Name, out count);

            if (card.Border == 1 && count >= 1)
            {
                return false;
            }

            if (card.Border == 0 && count >= 2)
            {
                return false;
            }

            return true;
        }

        /*
         * When a random card is selected,
         * Either update its count or add a new counter
         */
        private void UpdateCardCount(DefaultCard card)
        {
            if (CardCount.ContainsKey(card.Name))
            {
                CardCount[card.Name]++;
            }
            else
            {
                CardCount.Add(card.Name, 1);
            }
        }
    }
}

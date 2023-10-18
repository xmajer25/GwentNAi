
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Player
{
    public abstract class DefaultLeader
    {
        public int provisionValue { get; set; }
        public string leaderName { get; set; } = string.Empty;
        public string leaderFaction { get; set; } = string.Empty;
        public bool isStarting { get; set; }
        public int victories { get; set; }
        public int abilityCharges { get; set; }
        public int playerMethod { get; set; } // example: MCTS (0), Human(1)...
        public bool hasPassed { get; set; }
        public bool hasPlayedCard { get; set; }
        public bool hasUsedAbility { get; set; }
        public Deck startingDeck { get; set; } = new Deck();
        public Deck handDeck { get; set; } = new Deck();
        public Deck graveyardDeck { get; set; } = new Deck();

        public List<List<DefaultCard>> Board { get; set; } = new List<List<DefaultCard>>(2) { new List<DefaultCard>(10), new List<DefaultCard>(10) };

        private static Random Shuffler = new();


        public abstract void Order();
        public abstract void Update();

        public void ShuffleStartingDeck()
        {
            int n = startingDeck.Cards.Count;

            while (n > 1)
            {
                n--;
                int k = Shuffler.Next(n + 1);
                DefaultCard value = startingDeck.Cards[k];
                startingDeck.Cards[k] = startingDeck.Cards[n];
                startingDeck.Cards[n] = value;
            }
        }

        public void DrawCards(int numberOfCards)
        {
            for(int i = 0;  i < numberOfCards; i++)
            {
                if (handDeck.Cards.Count >= 10) break;
                if (startingDeck.Cards.Count <= 0) break;

                DefaultCard drawnCard = startingDeck.Cards.First();
                startingDeck.Cards.RemoveAt(0);
                handDeck.Cards.Add(drawnCard);
            }
        }

        public void PlayCard(int CardInHandIndex, int RowIndex, int PosIndex)
        {
            hasPlayedCard = true;
            Board[RowIndex].Insert(PosIndex, handDeck.Cards[CardInHandIndex]);
            handDeck.Cards.RemoveAt(CardInHandIndex);
        }

        public void Pass()
        {
            hasPassed = true;
        }

        public void EndTurn()
        {

        }

        public void UseAbility()
        {
            hasUsedAbility = true;
        }
    }
}

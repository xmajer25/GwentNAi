using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Cards.Neutral;
using GwentNAi.GameSource.Cards.Syndicate;

namespace GwentNAi.GameSource.Decks.DeckSeeds
{
    public class TestDeck : Deck
    {
        public TestDeck()
        {
            Cards = new()
            {
                new Griffin(), new Griffin(), new Griffin(), new Griffin(), new Griffin(),
                new Griffin(), new Griffin(), new Griffin(), new Griffin(), new Griffin(),
                new Griffin(), new Griffin(), new Griffin(), new Griffin(), new Griffin(),
                new Griffin(), new Griffin(), new Griffin(), new Griffin(), new Griffin(),
                new Griffin(), new Griffin(), new Griffin(), new Griffin(), new Griffin()
            };
            Name = "Test Deck";
        }
    }
}

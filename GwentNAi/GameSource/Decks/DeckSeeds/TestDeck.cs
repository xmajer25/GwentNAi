using GwentNAi.GameSource.Cards.Monsters;

namespace GwentNAi.GameSource.Decks.DeckSeeds
{
    public class TestDeck : DefaultDeck
    {
        public TestDeck()
        {
            Cards = new()
            {
                new IceGiant(), new IceGiant(), new IceGiant(), new IceGiant(), new IceGiant(),
                new IceGiant(), new IceGiant(), new IceGiant(), new IceGiant(), new IceGiant(),
                new IceGiant(), new IceGiant(), new IceGiant(), new IceGiant(), new IceGiant(),
                new Griffin(), new Griffin(), new Griffin(), new Griffin(), new Griffin(),
                new Griffin(), new Griffin(), new Griffin(), new Griffin(), new Griffin()
            };
            Name = "Test Deck";
        }
    }
}

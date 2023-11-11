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
                new OldSpeartipAsleep(), new OldSpeartipAsleep(), new OldSpeartipAsleep(), new OldSpeartipAsleep(), new OldSpeartipAsleep(),
                new OldSpeartipAsleep(), new OldSpeartipAsleep(), new OldSpeartipAsleep(), new GeraltProfessional(), new GeraltProfessional(),
                new GeraltProfessional(), new Brewess(), new Nekker(), new Nekker(), new Nekker(),
                new Nekker(), new Nekker(), new Nekker(), new Nekker(), new Nekker(),
                new Nekker(), new Nekker(), new Nekker(), new Nekker(), new Nekker()
            };
            Name = "Test Deck";
        }
    }
}

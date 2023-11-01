using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Cards.Neutral;
using GwentNAi.GameSource.Cards.Syndicate;

namespace GwentNAi.GameSource.Decks.DeckSeeds
{
    public class RenfriDeck : Deck
    {
        public RenfriDeck()
        {
            Cards = new()
            {
                new OldSpeartipAsleep(), new OldSpeartipAsleep(), new OldSpeartipAsleep(), new OldSpeartipAsleep(), new OldSpeartipAsleep(),
                new OldSpeartipAsleep(), new OldSpeartipAsleep(), new OldSpeartipAsleep(), new GeraltProfessional(), new GeraltProfessional(),
                new GeraltProfessional(), new Brewess(), new Protofleder(), new Protofleder(), new Protofleder(),
                new Protofleder(), new Protofleder(), new Protofleder(), new Protofleder(), new Protofleder(),
                new Protofleder(), new Protofleder(), new Protofleder(), new Protofleder(), new Protofleder()
            };
            Name = "Renfri";
        }
    }
}

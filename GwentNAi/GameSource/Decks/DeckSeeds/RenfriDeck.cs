using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Cards.Neutral;

namespace GwentNAi.GameSource.Decks.DeckSeeds
{
    public class RenfriDeck : Deck
    {
        public RenfriDeck()
        {
            Cards = new()
            {
                new Protofleder(), new Protofleder(), new Protofleder(), new Protofleder(), new Protofleder(),
                new Protofleder(), new Protofleder(), new GeraltProfessional(), new GeraltProfessional(), new GeraltProfessional(),
                new GeraltProfessional(), new GeraltProfessional(), new GeraltProfessional(), new OlgierVonEverec(), new OlgierVonEverec(),
                new Katakan(), new Katakan(), new Katakan(), new Katakan(), new Katakan(),
                new Katakan(), new Katakan(), new Katakan(), new Katakan(), new Katakan()
            };
            Name = "Renfri";
        }
    }
}

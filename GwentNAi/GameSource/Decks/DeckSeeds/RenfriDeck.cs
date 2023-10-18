using GwentNAi.GameSource.Cards.Neutral;

namespace GwentNAi.GameSource.Decks.DeckSeeds
{
    public class RenfriDeck : Deck
    {
        public RenfriDeck()
        {
            Cards = new()
            {
                new Renfri(), new Renfri(), new Renfri(), new Renfri(), new Renfri(),
                new Renfri(), new Renfri(), new GeraltProfessional(), new GeraltProfessional(), new GeraltProfessional(),
                new GeraltProfessional(), new GeraltProfessional(), new GeraltProfessional(), new OlgierVonEverec(), new OlgierVonEverec(),
                new OlgierVonEverec(), new OlgierVonEverec(), new OlgierVonEverec(), new OlgierVonEverec(), new OlgierVonEverec(),
                new OlgierVonEverec(), new OlgierVonEverec(), new OlgierVonEverec(), new OlgierVonEverec(), new OlgierVonEverec()
            };
            Name = "Renfri";
        }
    }
}

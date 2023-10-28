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
                new Whispess(), new Whispess(), new Whispess(), new Whispess(), new Whispess(),
                new Whispess(), new Whispess(), new GeraltProfessional(), new GeraltProfessional(), new GeraltProfessional(),
                new GeraltProfessional(), new Brewess(), new GeraltProfessional(), new OlgierVonEverec(), new OlgierVonEverec(),
                new Brewess(), new Brewess(), new Brewess(), new Brewess(), new Brewess(),
                new Brewess(), new Whispess(), new Whispess(), new Whispess(), new PugoBoomBreaker()
            };
            Name = "Renfri";
        }
    }
}

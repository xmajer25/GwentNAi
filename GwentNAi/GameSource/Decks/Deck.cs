using GwentNAi.GameSource.Cards;

namespace GwentNAi.GameSource.Decks
{
    public class Deck
    {
        public List<DefaultCard> Cards { get; set; } = new List<DefaultCard>();
        public string Name { get; set; } = string.Empty;
    }
}

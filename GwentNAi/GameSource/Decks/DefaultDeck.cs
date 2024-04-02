using GwentNAi.GameSource.Cards;

namespace GwentNAi.GameSource.Decks
{
    public class DefaultDeck
    {
        public List<DefaultCard> Cards { get; set; } = new List<DefaultCard>();
        public string Name { get; set; } = string.Empty;

        public object Copy()
        {
            DefaultDeck copyDeck = new DefaultDeck()
            {
                Cards = Cards.Select(card => (DefaultCard)card.Clone()).ToList(),
                Name = Name
            };

            return copyDeck;
        }
    }
}

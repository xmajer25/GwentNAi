using GwentNAi.GameSource.Cards;

namespace GwentNAi.GameSource.Decks
{
    public class Deck
    {
        public List<DefaultCard> Cards { get; set; } = new List<DefaultCard>();
        public string Name { get; set; } = string.Empty;

        public object Copy()
        {
            Deck copyDeck = new Deck()
            {
                Cards = Cards.Select(card => (DefaultCard)card.Clone()).ToList(),
                Name = Name
            };

            return copyDeck;
        }
    }
}

using GwentNAi.GameSource.Cards;

namespace GwentNAi.GameSource.Decks
{
    /*
     * Class for holding a collection of cards
     * used for defining decks used for gameplay
     * used for holding info about cards in hand, graveyard...
     */
    public class DefaultDeck
    {
        public List<DefaultCard> Cards { get; set; } = new List<DefaultCard>();
        public string Name { get; set; } = string.Empty;

        /*
         * Creates a deep clone of this object
         */
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

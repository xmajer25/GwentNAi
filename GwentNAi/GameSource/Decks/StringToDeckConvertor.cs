using GwentNAi.GameSource.CustomExceptions;
using GwentNAi.GameSource.Decks.DeckSeeds;

namespace GwentNAi.GameSource.Decks
{
    /*
     * Converter from user input to deck objects
     */
    public static class StringToDeckConvertor
    {
        public static DefaultDeck Convert(string? Deck)
        {
            if (string.IsNullOrWhiteSpace(Deck))
                return null;

            if (Deck.Equals("SeedDeck1", StringComparison.OrdinalIgnoreCase) || Deck.Equals("1", StringComparison.OrdinalIgnoreCase))
                return new SeedDeck1();
            else if (Deck.Equals("SeedDeck2", StringComparison.OrdinalIgnoreCase) || Deck.Equals("2", StringComparison.OrdinalIgnoreCase))
                return new SeedDeck2();
            else
                return null;
        }
    }
}

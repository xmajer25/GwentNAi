using GwentNAi.GameSource.CustomExceptions;
using GwentNAi.GameSource.Decks.DeckSeeds;

namespace GwentNAi.GameSource.Decks
{
    public static class StringToDeckConvertor
    {
        public static Deck Convert(string ?Deck)
        {
            switch (Deck)
            {
                case "Renfri":
                case "renfri":
                case "1":
                    return new RenfriDeck();
                default:
                    throw new CustomException("Error: Unknown Deck Name");

            }
        }
    }
}

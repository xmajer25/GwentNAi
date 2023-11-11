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
                case "TestDeck":
                case "testdeck":
                case "1":
                    return new TestDeck();
                case "MonsterDeck1":
                case "monsterdeck1":
                case "2":
                    return new MonsterDeck1();
                default:
                    throw new CustomException("Error: Unknown Deck Name");

            }
        }
    }
}

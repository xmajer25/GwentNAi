using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Cards.Neutral
{
    public class Renfri : DefaultCard
    {
        public Renfri()
        {
            CurrentValue = 5;
            MaxValue = 5;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "neutral";
            Name = "Renfri";
            ShortName = "Renfri";
            Descriptors = new List<string>() { "Human", "Cursed", "Bandit" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        public static void Deploy(DefaultDeck deck)
        {
            throw new NotImplementedException();
        }
    }
}

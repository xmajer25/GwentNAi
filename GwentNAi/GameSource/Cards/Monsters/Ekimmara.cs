using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Ekimmara : DefaultCard, IDoomed
    {
        public Ekimmara()
        {
            CurrentValue = 3;
            MaxValue = 3;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Ekimmara";
            ShortName = "Ekimmara";
            Descriptors = new List<string>() { "Vampire", "Token" };
            TimeToOrder = 0;
            Bleeding = 0;
        }
    }
}

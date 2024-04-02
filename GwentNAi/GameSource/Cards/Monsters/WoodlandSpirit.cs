using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class WoodlandSpirit : DefaultCard, IDoomed
    {
        public WoodlandSpirit()
        {
            CurrentValue = 9;
            MaxValue = 9;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "WoodlandSpirit";
            ShortName = "Woodland";
            Descriptors = new List<string>() { "Relict", "Token" };
            TimeToOrder = -1;
            Bleeding = 0;
        }
    }
}

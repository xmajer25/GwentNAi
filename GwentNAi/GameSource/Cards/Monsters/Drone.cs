using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Drone : DefaultCard, IDoomed
    {
        public Drone()
        {
            CurrentValue = 1;
            MaxValue = 1;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Drone";
            ShortName = "Drone";
            Descriptors = new List<string>() { "Insectoid", "Token" };
            TimeToOrder = -1;
            Bleeding = 0;
        }
    }
}

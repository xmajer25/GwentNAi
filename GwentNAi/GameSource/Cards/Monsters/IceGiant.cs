namespace GwentNAi.GameSource.Cards.Monsters
{
    public class IceGiant : DefaultCard
    {
        public IceGiant()
        {
            CurrentValue = 7;
            MaxValue = 7;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Ice Giant";
            ShortName = "IceGiant";
            Descriptors = new List<string>() { "Ogroid" };
            TimeToOrder = 0;
            Bleeding = 0;
        }
    }
}

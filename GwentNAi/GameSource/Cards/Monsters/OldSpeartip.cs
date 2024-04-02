namespace GwentNAi.GameSource.Cards.Monsters
{
    public class OldSpeartip : DefaultCard
    {
        public OldSpeartip()
        {
            CurrentValue = 12;
            MaxValue = 12;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "OdlSpeartip";
            ShortName = "Speartip";
            Descriptors = new List<string>() { "Ogroid" };
            TimeToOrder = 0;
            Bleeding = 0;
        }
    }
}

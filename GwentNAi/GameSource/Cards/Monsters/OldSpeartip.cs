namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class OldSpeartip : DefaultCard
    {
        /*
         * Initialize information about specific card 
         */
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

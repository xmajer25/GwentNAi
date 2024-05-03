namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class IceGiant : DefaultCard
    {
        /*
         * Initialize information about specific card 
         */
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

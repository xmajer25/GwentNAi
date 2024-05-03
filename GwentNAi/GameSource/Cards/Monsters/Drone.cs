using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Drone : DefaultCard, IDoomed
    {
        /*
         * Initialize information about specific card 
         */
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

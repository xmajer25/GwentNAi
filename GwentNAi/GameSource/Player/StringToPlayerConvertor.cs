using GwentNAi.GameSource.CustomExceptions;
using GwentNAi.GameSource.Player.Monsters;


namespace GwentNAi.GameSource.Player
{
    /*
     * Converter from user input into a leader object
     */
    public static class StringToPlayerConvertor
    {
        public static DefaultLeader Convert(string? PlayerName)
        {
            switch (PlayerName)
            {
                case "ArachasSwarm":
                case "arachasswarm":
                case "Arachasswarm":
                case "1":
                    return new ArachasSwarm();
                case "BloodScent":
                case "bloodscent":
                case "Bloodscent":
                case "2":
                    return new BloodScent();
                case "ForceOfNature":
                case "Forceofnature":
                case "forceofnature":
                case "3":
                    return new ForceOfNature();
                default:
                    throw new CustomException("Error: Unknown Player Name");
            }
        }
    }
}

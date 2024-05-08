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
            if (string.IsNullOrWhiteSpace(PlayerName))
                return null;

            if (PlayerName.Equals("ArachasSwarm", StringComparison.OrdinalIgnoreCase) || PlayerName.Equals("1", StringComparison.OrdinalIgnoreCase))
                return new ArachasSwarm();
            else if (PlayerName.Equals("ForceOfNature", StringComparison.OrdinalIgnoreCase) || PlayerName.Equals("3", StringComparison.OrdinalIgnoreCase))
                return new ForceOfNature();
            else
                return null;
        }

    }
}

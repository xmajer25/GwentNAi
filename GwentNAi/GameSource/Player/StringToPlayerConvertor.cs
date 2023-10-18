using GwentNAi.GameSource.CustomExceptions;
using GwentNAi.GameSource.Player.Monsters;


namespace GwentNAi.GameSource.Player
{
    public static class StringToPlayerConvertor
    {
        public static DefaultLeader Convert(string ?PlayerName)
        {
            switch(PlayerName)
            {
                case "ArachasSwarm":
                case "arachasswarm":
                case "Arachasswarm":
                    return new ArachasSwarm();
                case "BloodScent":
                case "bloodscent":
                case "Bloodscent":
                    return new BloodScent();
                default:
                    throw new CustomException("Error: Unknown Player Name");
            }
        }
    }
}

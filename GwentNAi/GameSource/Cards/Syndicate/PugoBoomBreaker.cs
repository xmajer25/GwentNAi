using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Syndicate
{
    public class PugoBoomBreaker : DefaultCard, IDeploy
    {
        public PugoBoomBreaker()
        {
            CurrentValue = 11;
            MaxValue = 11;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "neutral";
            Name = "Pugo Boom-Breaker";
            ShortName = "PugoBoom";
            Descriptors = new List<string>() { "ogroid", "cutups" };
            TimeToOrder = 1;
            Bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> currentBoard = board.GetCurrentBoard();
            if (currentBoard[0].Count == 0 && currentBoard[1].Count == 0)
            {
                TakeDemage(5, false, board);
                return;
            }

            Random random = new Random();
            int randomRowIndex = random.Next(0, currentBoard.Count);
            if (currentBoard[randomRowIndex].Count == 0)
            {
                randomRowIndex = (randomRowIndex == 0 ? 1 : 0);
            }

            int randomColumnIndex = random.Next(0, currentBoard[randomRowIndex].Count);
            currentBoard[randomRowIndex][randomColumnIndex].TakeDemage(3, false, board);
        }
    }
}

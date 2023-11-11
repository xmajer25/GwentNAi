using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GwentNAi.GameSource.Cards.Syndicate
{
    public class PugoBoomBreaker : DefaultCard, IDeploy
    {
        public PugoBoomBreaker()
        {
            currentValue = 11;
            maxValue = 11;
            shield = 0;
            provisionValue = 7;
            border = 1;
            type = "unit";
            faction = "neutral";
            name = "Pugo Boom-Breaker";
            shortName = "PugoBoom";
            descriptors = new List<string>() { "ogroid", "cutups" };
            timeToOrder = 1;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> currentBoard = board.CurrentlyPlayingLeader.Board;
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

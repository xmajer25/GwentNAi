using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Weavess : DefaultCard, IDeploy, IDeployExpandPickAlly, ICroneInteraction
    {
        int boost = 2;
        public Weavess()
        {
            currentValue = 6;
            maxValue = 6;
            shield = 0;
            provisionValue = 7;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Weavess";
            shortName = "Weavess";
            descriptors = new List<string>() { "Relict", "Crone" };
            timeToOrder = -1;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<int>> allyIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            List<List<DefaultCard>> allyBoard;
            allyBoard = board.CurrentPlayerBoard;
            int currentRow = 0;
            int currentIndex = 0;

            foreach (var row in allyBoard)
            {
                foreach (var card in row)
                {
                    if (card == this)
                    {
                        currentIndex++;
                        continue;
                    }
                    allyIndexes[currentRow].Add(currentIndex);
                    currentIndex++;
                }
                currentIndex = 0;
                currentRow++;
            }

            board.CurrentPlayerActions.ImidiateActions[0] = allyIndexes;
        }

        public void PostPickAllyAbilitiy(GameBoard board, int row, int index)
        {
            DefaultCard boostedCard = board.CurrentPlayerBoard[row][index];
            boostedCard.currentValue += boost;
            boostedCard.maxValue += boost;
        }

        public void RespondToCrone()
        {
            boost += 2;
        }
    }
}

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
            List<List<DefaultCard>> allyBoard = board.GetCurrentBoard();
            int currentRow = 0;

            foreach (var row in allyBoard)
            {
                for(int currentIndex = 0; currentIndex < row.Count; currentIndex++)
                {
                    DefaultCard card = row[currentIndex];
                    if(card == this)
                    {
                        continue;
                    }
                    board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentIndex);
                }
                currentRow++;
            }
        }

        public void PostPickAllyAbilitiy(GameBoard board, int row, int index)
        {
            DefaultCard boostedCard = board.GetCurrentBoard()[row][index];
            boostedCard.currentValue += boost;
            boostedCard.maxValue += boost;
        }

        public void RespondToCrone()
        {
            boost += 2;
        }
    }
}

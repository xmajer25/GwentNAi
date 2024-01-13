using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Brewess : DefaultCard, ICroneInteraction, IOrder, IOrderExpandPickAlly, ICharge
    {
        public int charge = 1;
        public Brewess()
        {
            currentValue = 6;
            maxValue = 6;
            shield = 0;
            provisionValue = 7;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Brewess";
            shortName = "Brewess";
            descriptors = new List<string>() { "Relict", "Crone" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public void Order(GameBoard board)
        {
            if (charge <= 0) return;
            List<List<DefaultCard>> currentBoard = board.GetCurrentBoard();
            int currentRow = 0;

            foreach (var row in currentBoard)
            {
                for(int currentIndex = 0;  currentIndex < row.Count; currentIndex++) 
                {
                    DefaultCard card = row[currentIndex];
                    if (card == this)
                    {
                        continue;
                    }
                    board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentIndex);
                }
                currentRow++;
            }
        }

        public void PostPickAllyOrder(GameBoard board, int row, int index)
        {
            DefaultCard consumedCard = board.GetCurrentBoard()[row][index];
            currentValue += consumedCard.currentValue;
            maxValue += consumedCard.currentValue;
            consumedCard.TakeDemage(consumedCard.currentValue, true, board);
            charge--;
        }

        public void RespondToCrone()
        {
            charge++;
        }
    }
}

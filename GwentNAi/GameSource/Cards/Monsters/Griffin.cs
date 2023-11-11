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
    public class Griffin : DefaultCard, IDeploy, IDeployExpandPickAlly
    {
        public Griffin()
        {
            currentValue = 9;
            maxValue = 9;
            shield = 0;
            provisionValue = 5;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "Griffin";
            shortName = "Griffin";
            descriptors = new List<string>() { "Beast" };
            timeToOrder = 0;
            bleeding = 0;
        }
        public void Deploy(GameBoard board)
        {
            List<List<int>> allyIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            int currentRow = 0;
            int currentIndex = 0;

            foreach (var row in board.CurrentPlayerBoard)
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
            if (allyIndexes[0].Count == 0 && allyIndexes[1].Count == 0)
            {
                this.TakeDemage(currentValue, true, board);
                return;
            }
            board.CurrentPlayerActions.ImidiateActions[0] = allyIndexes;
        }

        public void PostPickAllyAbilitiy(GameBoard board, int row, int index)
        {
            DefaultCard DestroyedAlly = board.CurrentPlayerBoard[row][index];
            DestroyedAlly.TakeDemage(DestroyedAlly.currentValue, true, board);
        }
    }
}

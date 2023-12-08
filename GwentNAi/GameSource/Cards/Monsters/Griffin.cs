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
            List<List<DefaultCard>> AllyBoard = board.GetCurrentBoard();
            int currentIndex = 0;
            bool isAllyPresent = false;
            for (int row = 0; row < AllyBoard.Count; row++)
            {   
                foreach (var card in AllyBoard[row])
                {
                    if (card == this)
                    {
                        currentIndex++;
                        continue;
                    }
                    isAllyPresent = true;
                    board.CurrentPlayerActions.ImidiateActions[0][row].Add(currentIndex);
                    currentIndex++;
                }
                currentIndex = 0;
            }
            if (!isAllyPresent)
            {
                this.TakeDemage(currentValue, true, board);
                return;
            }
        }

        public void PostPickAllyAbilitiy(GameBoard board, int row, int index)
        {
            DefaultCard DestroyedAlly = board.GetCurrentBoard()[row][index];
            DestroyedAlly.TakeDemage(DestroyedAlly.currentValue, true, board);
        }
    }
}

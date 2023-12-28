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
            bool isAllyPresent = false;
            int row = GetCurrentRow(AllyBoard);
            
            for (int card = 0; card < AllyBoard[row].Count; card++)
            {
                if (AllyBoard[row][card] == this)
                {
                    continue;
                }
                isAllyPresent = true;
                board.CurrentPlayerActions.ImidiateActions[0][row].Add(card);
            }
            
            if (!isAllyPresent)
            {
                board.CurrentPlayerActions.ClearImidiateActions();
                this.TakeDemage(currentValue, true, board);
                return;
            }
        }

        public int GetCurrentRow(List<List<DefaultCard>> AllyBoard)
        {
            int isInRow = AllyBoard[0].IndexOf(this);
            return (isInRow == -1) ? 1 : 0;
        }

        public void PostPickAllyAbilitiy(GameBoard board, int row, int index)
        {
            DefaultCard DestroyedAlly = board.GetCurrentBoard()[row][index];
            DestroyedAlly.TakeDemage(DestroyedAlly.currentValue, true, board);
        }
    }
}

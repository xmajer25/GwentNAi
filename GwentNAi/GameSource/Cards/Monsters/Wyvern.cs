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
    public class Wyvern : DefaultCard, IDeploy, IDeployExpandPickEnemies, IThrive
    {
        public Wyvern()
        {
            currentValue = 3;
            maxValue = 3;
            shield = 0;
            provisionValue = 5;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "Wyvern";
            shortName = "Wyvern";
            descriptors = new List<string>() { "Beast" };
            timeToOrder = -1;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();

            for (int currentRow = 0; currentRow < enemieBoard.Count; currentRow++)
            {
                for (int currentIndex = 0; currentIndex < enemieBoard[currentRow].Count; currentIndex++)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentIndex);
                }  
            }
        }

        public void postPickEnemieAbilitiy(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            enemieBoard[row][index].TakeDemage(2, false, board);
        }

        public void Thrive(int playedUnitValue)
        {
            if(playedUnitValue > currentValue)
            {
                currentValue++;
                if (currentValue > maxValue) maxValue++;

            }
        }
    }
}

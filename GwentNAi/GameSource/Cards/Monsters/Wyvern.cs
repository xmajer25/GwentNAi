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
            List<List<int>> enemieIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            int currentRow = 0;
            int currentIndex = 0;

            foreach (var row in enemieBoard)
            {
                foreach (var card in row)
                {
                    enemieIndexes[currentRow].Add(currentIndex);
                    currentIndex++;
                }
                currentIndex = 0;
                currentRow++;
            }

            board.CurrentPlayerActions.ImidiateActions[0] = enemieIndexes;
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

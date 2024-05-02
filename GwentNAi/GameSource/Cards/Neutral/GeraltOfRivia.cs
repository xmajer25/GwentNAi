using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Neutral
{
    public class GeraltOfRivia : DefaultCard, IDeploy, IDeployExpandPickEnemies
    {
        public GeraltOfRivia()
        {
            CurrentValue = 3;
            MaxValue = 3;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "neutral";
            Name = "GeraltOfRivia";
            ShortName = "GeraltOR";
            Descriptors = new List<string>() { "Witcher" };
            TimeToOrder = -1;
            Bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();

            for (int currentRow = 0; currentRow < enemieBoard.Count; currentRow++)
            {
                for (int currentIndex = 0; currentIndex < enemieBoard[currentRow].Count; currentIndex++)
                {
                    if (enemieBoard[currentRow][currentIndex].CurrentValue >= 9) board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentIndex);
                }
            }
        }

        public void postPickEnemieAbilitiy(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            enemieBoard[row][index].TakeDemage(9, true, board);
        }
    }
}

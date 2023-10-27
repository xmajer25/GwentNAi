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
    public class Protofleder : DefaultCard, IDeploy, IDeployExpandPickEnemies
    {
        public Protofleder()
        {
            currentValue = 4;
            maxValue = 4;
            provisionValue = 9;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Protofleder";
            shortName = "Protofl.";
            descriptors = new List<string>() { "Vampire"};
            timeToOrder = 0;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<int>> enemieIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            List<List<DefaultCard>> enemieBoard;
            enemieBoard = (board.CurrentPlayerBoard == board.Leader1.Board ? board.Leader2.Board : board.Leader1.Board);
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
            List<List<DefaultCard>> enemieBoard = (board.CurrentPlayerBoard == board.Leader1.Board ? board.Leader2.Board : board.Leader1.Board);
            enemieBoard[row][index].bleeding += 3;
            
            currentValue += enemieBoard[row][index].bleeding;
            maxValue = maxValue - currentValue + enemieBoard[row][index].bleeding;
        }
    }
}

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
    public class Whispess : DefaultCard, IDeploy, IDeployExpandPickEnemies, ICroneInteraction
    {
        private int abilityDemage { get; set; } = 2;
        public Whispess()
        {
            currentValue = 6;
            maxValue = 6;
            shield = 0;
            provisionValue = 7;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Whispess";
            shortName = "Whispess";
            descriptors = new List<string>() { "Relict", "Crone" };
            timeToOrder = -1;
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
            enemieBoard[row][index].currentValue -= abilityDemage;
        }

        public void RespondToCrone()
        {
            abilityDemage += 2;
        }
    }
}

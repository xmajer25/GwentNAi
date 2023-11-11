using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class WildHuntHound : DefaultCard, IEndTurnUpdate
    {
        public WildHuntHound()
        {
            currentValue = 3;
            maxValue = 3;
            shield = 0;
            provisionValue = 4;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "WildHuntHound";
            shortName = "WH.Hound";
            descriptors = new List<string>() { "Beast", "Wild Hunt"};
            timeToOrder = -1;
            bleeding = 0;
        }

        public void EndTurnUpdate(GameBoard board)
        {
            if(!IsDominant(board)) return;

            currentValue++;
            if (currentValue >= maxValue) maxValue++;
        }

        private bool IsDominant(GameBoard board)
        {
            List<List<DefaultCard>> enemiePlayerBoard = (board.CurrentPlayerBoard == board.Leader1.Board ? board.Leader2.Board : board.Leader1.Board);
            DefaultCard currentMax = board.CurrentPlayerBoard
                .SelectMany(list => list)
                .OrderByDescending(obj => obj.currentValue)
                .FirstOrDefault();

            DefaultCard enemieMax = enemiePlayerBoard
                .SelectMany(list => list)
                .OrderByDescending(obj => obj.currentValue)
                .FirstOrDefault();

            if (enemieMax == null) return true;
            if (currentMax != null)
            {
                return currentMax.currentValue >= enemieMax.currentValue;
            }
            return true;
        }
    }
}

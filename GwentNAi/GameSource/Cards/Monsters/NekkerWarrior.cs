using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class NekkerWarrior : DefaultCard, IDeploy, IThrive
    {
        private bool hasTriggeredThrive;
        public NekkerWarrior()
        {
            currentValue = 7;
            maxValue = 7;
            shield = 0;
            provisionValue = 4;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "NekkerWarrior";
            shortName = "Nekker:W";
            descriptors = new List<string>() { "Ogroid", "Warrior" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            hasTriggeredThrive = board.CurrentPlayerBoard[0].Any(obj => obj is IThrive && obj.currentValue < currentValue);
            if(!hasTriggeredThrive)
            {
                hasTriggeredThrive = board.CurrentPlayerBoard[1].Any(obj => obj is IThrive && obj.currentValue < currentValue);
            }
            if (!hasTriggeredThrive)
            {
                TakeDemage(3, false, board);
            }
        }

        public void Thrive(int playedUnitValue)
        {
            if(!hasTriggeredThrive)
            {
                if (playedUnitValue > currentValue)
                {
                    currentValue++;
                    if (currentValue > maxValue) maxValue++;

                }
            }
        }
    }
}

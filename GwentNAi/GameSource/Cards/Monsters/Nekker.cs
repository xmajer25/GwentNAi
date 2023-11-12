﻿using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Nekker : DefaultCard, IDeploy, IThrive
    {
        public Nekker()
        {
            currentValue = 1;
            maxValue = 1;
            shield = 0;
            provisionValue = 4;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "Nekker";
            shortName = "Nekker";
            descriptors = new List<string>() { "Ogroid" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            int thisIndex = board.CurrentPlayerBoard[0].IndexOf((DefaultCard)this);
            int thisRow = 0;
            if (thisIndex == -1)
            {
                thisIndex = board.CurrentPlayerBoard[1].IndexOf((DefaultCard)this);
                thisRow = 1;
            }

            if (board.CurrentPlayerBoard[thisRow].Count != 9)
            {
                board.CurrentPlayerBoard[thisRow].Insert(thisIndex + 1, new Nekker());
            }
        }

        public void Thrive(int playedUnitValue)
        {
            if (playedUnitValue > currentValue)
            {
                currentValue++;
                if (currentValue > maxValue) maxValue++;

            }
        }
    }
}
﻿using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Brewess : DefaultCard, ICroneInteraction, IOrder, IOrderExpandPickAlly, ICharge
    {
        public int charge = 1;
        public Brewess()
        {
            currentValue = 6;
            maxValue = 6;
            shield = 0;
            provisionValue = 7;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Brewess";
            shortName = "Brewess";
            descriptors = new List<string>() { "Relict", "Crone" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public void Order(GameBoard board)
        {
            List<List<int>> allyIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            List<List<DefaultCard>> currentBoard = board.CurrentPlayerBoard;
            int currentRow = 0;
            int currentIndex = 0;

            foreach (var row in currentBoard)
            {
                foreach (var card in row)
                {
                    if(card == this)
                    {
                        currentIndex++;
                        continue;
                    }
                    allyIndexes[currentRow].Add(currentIndex);
                    currentIndex++;
                }
                currentIndex = 0;
                currentRow++;
            }

            board.CurrentPlayerActions.ImidiateActions[0] = allyIndexes;
        }

        public void PostPickAllyOrder(GameBoard board, int row, int index)
        {
            DefaultCard consumedCard = board.CurrentPlayerBoard[row][index];
            currentValue += consumedCard.currentValue;
            maxValue += consumedCard.currentValue;
            consumedCard.currentValue = 0;
            charge--;
        }

        public void RespondToCrone()
        {
            charge++;
        }
    }
}

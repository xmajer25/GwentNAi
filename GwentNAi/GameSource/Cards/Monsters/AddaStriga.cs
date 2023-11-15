using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IExpand;
using GwentNAi.GameSource.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class AddaStriga : DefaultCard, IOrder, IOrderExpandPickAll
    {
        public AddaStriga()
        {
            currentValue = 6;
            maxValue = 6;
            shield = 0;
            provisionValue = 9;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Adda:Striga";
            shortName = "Adda";
            descriptors = new List<string>() { "Beast", "Cursed" };
            timeToOrder = 0;
            bleeding = 0;
        }
        public void Order(GameBoard board)
        {
            pickAll(board);
        }

        public void pickAll(GameBoard board)
        {
            List<List<int>> p1Rows = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            List<List<int>> p2Rows = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            int currentRow = 0;
            int currentColumn = 0;


            foreach (var row in board.Leader1.Board)
            {
                foreach (var card in row)
                {
                    if (card.currentValue < this.currentValue && card.descriptors.Contains("Token"))
                    {
                        p1Rows[currentRow].Add(currentColumn);
                    }
                    currentColumn++;
                }
                currentRow++;
                currentColumn = 0;
            }

            currentRow = currentColumn = 0;

            foreach (var row in board.Leader2.Board)
            {
                foreach (var card in row)
                {
                    if (card.currentValue < this.currentValue && card.descriptors.Contains("Token"))
                    {
                        p2Rows[currentRow].Add(currentColumn);
                    }
                    currentColumn++;
                }
                currentRow++;
                currentColumn = 0;
            }

            board.CurrentPlayerActions.ImidiateActions[0] = p1Rows;
            board.CurrentPlayerActions.ImidiateActions[1] = p2Rows;
        }

        void IOrderExpandPickAll.postPickAllOrder(GameBoard board, int player, int row, int index)
        {
            int multiplier = 1;
            List<List<DefaultCard>> targetedBoard = (player == 0 ? board.Leader1.Board : board.Leader2.Board);
            DefaultLeader targetedLeader = (player == 0 ? board.Leader1 : board.Leader2);
            if (targetedLeader == board.CurrentlyPlayingLeader) multiplier = 2;
            DefaultCard consumedCard = targetedBoard[row][index];

            currentValue += consumedCard.currentValue * multiplier;
            maxValue += consumedCard.currentValue * multiplier;
            consumedCard.TakeDemage(consumedCard.currentValue, true, board);
            timeToOrder--;
            board.CurrentlyPlayingLeader.UseAbility();
        }
    }
}

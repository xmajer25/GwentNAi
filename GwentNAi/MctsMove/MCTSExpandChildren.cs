using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.MctsMove
{
    public static class MCTSExpandChildren
    {
        public static void SwapCard(MCTSNode parent)
        {
            //Children for all possible swaps
            foreach(int index in parent.Board.CurrentPlayerActions.SwapCards.Indexes.ToList())
            {
                GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
                clonedBoard.CurrentlyPlayingLeader.SwapCards(index);
                clonedBoard.CurrentPlayerActions.SwapCards.CardSwaps--;
                parent.AppendChild(clonedBoard, false);
            }

            //Child where no swap has been chosen
            GameBoard noSwapBoard = (GameBoard)parent.Board.Clone();
            noSwapBoard.CurrentPlayerActions.SwapCards.StopSwapping = true;
            parent.AppendChild(noSwapBoard, true);
        }
    }
}

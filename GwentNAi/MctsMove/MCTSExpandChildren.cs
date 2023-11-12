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
        static void SwapCard(MCTSNode parent, GameBoard board)
        {
            foreach (int index in board.CurrentPlayerActions.ImidiateActions[0][0])
            {
                GameBoard boardClone = (GameBoard)board.Clone();
                boardClone.CurrentlyPlayingLeader.SwapCards(index);
                parent.AppendChild(boardClone);
            }
        }
    }
}

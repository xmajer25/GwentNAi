using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
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

        public static void PlayCard(MCTSNode parent) 
        {
            ActionContainer actionContainer = parent.Board.CurrentPlayerActions;

            for (int cardIndex = 0; cardIndex < actionContainer.PlayCardActions.Count; cardIndex++)
            {
                DefaultCard playCard = actionContainer.PlayCardActions[cardIndex].ActionCard;
                playCard.PlaySelfExpand(parent.Board);

                for (int row = 0; row < actionContainer.ImidiateActions[0].Count; row++)
                {
                    if (actionContainer.ImidiateActions[0][row].Count == 9) continue;
                    foreach(int index in actionContainer.ImidiateActions[0][row])
                    {
                        GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
                        clonedBoard.CurrentPlayerActions.ClearImidiateActions();
                        clonedBoard.CurrentPlayerActions.PlayCardActions.Clear();
                        clonedBoard.CurrentlyPlayingLeader.PlayCard(cardIndex, row, index, clonedBoard);
                        parent.AppendChild(clonedBoard, false);
                    }
                }
            }
            if (actionContainer.canPass)
            {
                GameBoard passBoard = (GameBoard)parent.Board.Clone();
                passBoard.CurrentPlayerActions.GetPassOrEndTurn(passBoard.CurrentlyPlayingLeader);
                passBoard.CurrentPlayerActions.PassOrEndTurn();
                parent.AppendChild(passBoard, true);
            }
            if (actionContainer.canEnd)
            {
                GameBoard endBoard = (GameBoard)parent.Board.Clone();
                parent.AppendChild(endBoard, true);
            }
        }
    }
}

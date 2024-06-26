﻿
using GwentNAi.GameSource.Board;

namespace GwentNAi.HumanMove
{
    /*
     * Class containing main logic behind human player
     * Listing possible actions, obtaining input, playing out the moves
     */
    public static class HumanPlayer
    {

        /*
         * Method called by the game to obtain the human player's move
         * Lists actions, obtains input, plays out the moves
         * Returns -1 if the move ended the player's turn
         * Returns 0 if player keeps playing after this action
         */
        public static int HumanMove(GameBoard board)
        {
            ActionContainer actionContainer = board.CurrentPlayerActions;

            if (SwapCards(board) == -1) return -1;
            if (board.GetCurrentLeader().HasPassed) return -1;

            HumanConsolePrint.swapColor(board);
            int[] maxActionId = HumanConsolePrint.ListActions(actionContainer);
            string action = HumanConsoleGet.GetHumanAction(maxActionId, actionContainer.CanPass, actionContainer.CanEnd, (actionContainer.LeaderActions != null));
            if (HumanStringToAction.Convert(action, board) == -1) return -1;
            return 0;
        }

        /*
         * Method for swapping cards at the start of the turn
         * If player can swap -> up to 3 swaps are handled 
         * Returns -1 if player was swapping (this ends the turn)
         * Returns 0 if player didn't have the option to swap
         */
        private static int SwapCards(GameBoard board)
        {
            ActionContainer actionContainer = board.CurrentPlayerActions;
            int cardSwapped;


            if (actionContainer.CardSwaps.SwapAvailable)
            {
                HumanConsolePrint.swapColor(board);
                while (actionContainer.CardSwaps.Indexes.Count != 0)
                {
                    HumanConsolePrint.ListCardsForSwapping(actionContainer.CardSwaps.Indexes, board.GetCurrentLeader().Hand, "Swap cards in hand");
                    cardSwapped = HumanConsoleGet.GetIndex(actionContainer.CardSwaps.Indexes);
                    if (cardSwapped == -1)
                    {
                        actionContainer.CardSwaps.StopSwapping = true;
                        continue;
                    }
                    actionContainer.CardSwaps.CardSwaps--;
                    board.GetCurrentLeader().SwapCards(cardSwapped);
                }
                return -1;
            }
            return 0;
        }
    }
}

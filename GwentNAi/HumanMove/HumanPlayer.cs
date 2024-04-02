
using GwentNAi.GameSource.Board;

namespace GwentNAi.HumanMove
{
    public static class HumanPlayer
    {
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

        private static int SwapCards(GameBoard board)
        {
            ActionContainer actionContainer = board.CurrentPlayerActions;
            int cardSwapped;


            if (actionContainer.CardSwaps.SwapAvailable)
            {
                HumanConsolePrint.swapColor(board);
                while (actionContainer.CardSwaps.Indexes.Count != 0)
                {
                    HumanConsolePrint.ListCards(actionContainer.CardSwaps.Indexes, board.GetCurrentLeader().Hand, "Swap cards in hand");
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

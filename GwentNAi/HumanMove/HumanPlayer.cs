
using GwentNAi.GameSource.Board;

namespace GwentNAi.HumanMove
{
    public static class HumanPlayer
    {
        public static int HumanMove(GameBoard board)
        {
            ActionContainer actionContainer = board.CurrentPlayerActions;

            if (SwapCards(board) == -1) return -1;
            if (board.CurrentlyPlayingLeader.hasPassed) return -1;
            if (board.CurrentlyPlayingLeader.hasPlayedCard) board.CurrentPlayerActions.PlayCardActions.Clear();

            HumanConsolePrint.swapColor(board);
            int[] maxActionId = HumanConsolePrint.ListActions(actionContainer);
            string action = HumanConsoleGet.GetHumanAction(maxActionId, actionContainer.canPass, actionContainer.canEnd, (actionContainer.LeaderActions != null));
            if (HumanStringToAction.Convert(action, board) == -1) return -1; 
            return 0;
        }

        private static int SwapCards(GameBoard board)
        {
            ActionContainer actionContainer = board.CurrentPlayerActions;
            int cardSwapped;


            if (actionContainer.SwapCards.SwapAvailable)
            {
                HumanConsolePrint.swapColor(board);
                while(actionContainer.SwapCards.Indexes.Count != 0)
                {
                    HumanConsolePrint.ListCards(actionContainer.SwapCards.Indexes, board.CurrentlyPlayingLeader.handDeck, "Swap cards in hand");
                    cardSwapped = HumanConsoleGet.GetIndex(actionContainer.SwapCards.Indexes);
                    if (cardSwapped == -1)
                    {
                        actionContainer.SwapCards.StopSwapping = true;
                        continue;
                    }
                    actionContainer.SwapCards.CardSwaps--;
                    board.CurrentlyPlayingLeader.SwapCards(cardSwapped);
                }
                return -1;
            }
            return 0;
        }
    }
}

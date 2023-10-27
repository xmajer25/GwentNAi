
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
            int numberOfCardsSwapped = 0;


            if (actionContainer.AreImidiateActionsFull())
            {
                HumanConsolePrint.swapColor(board);
                while(numberOfCardsSwapped < 3)
                {
                    HumanConsolePrint.ListCards(actionContainer.ImidiateActions[0][0], board.CurrentlyPlayingLeader.handDeck, "Swap cards in hand");
                    cardSwapped = HumanConsoleGet.GetIndex(actionContainer.ImidiateActions[0][0]);
                    if (cardSwapped == -1) return - 1;
                    numberOfCardsSwapped++;
                    board.CurrentlyPlayingLeader.SwapCards(cardSwapped);
                }
                board.CurrentPlayerActions.ClearImidiateActions();
                return -1;
            }
            return 0;
        }
    }
}

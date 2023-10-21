
using GwentNAi.GameSource.Board;

namespace GwentNAi.HumanMove
{
    public static class HumanPlayer
    {
        public static int HumanMove(GameBoard board)
        {
            ActionContainer actionContainer = board.CurrentPlayerActions;

            if (board.CurrentlyPlayingLeader.hasPassed) return -1;
            if (board.CurrentlyPlayingLeader.hasPlayedCard) board.CurrentPlayerActions.PlayCardActions.Clear();

            HumanConsolePrint.swapColor(board);
            int[] maxActionId = HumanConsolePrint.ListActions(actionContainer);
            string action = HumanConsoleGet.GetHumanAction(maxActionId, actionContainer.canPass, actionContainer.canEnd);
            if (HumanStringToAction.Convert(action, board) == -1) return -1; 
            return 0;
        }
    }
}

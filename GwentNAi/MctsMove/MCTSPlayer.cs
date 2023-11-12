using GwentNAi.GameSource.Board;

namespace GwentNAi.MctsMove
{
    public static class MCTSPlayer
    {
        public static int MCTSMove(GameBoard board)
        {
            GameBoard clonedBoard = (GameBoard)board.Clone();   
            MCTSNode Root = new MCTSNode(null, clonedBoard);

            if(board.CurrentPlayerActions.AreImidiateActionsFull())
            {
                //swap mcts
                //swap enemie
            }
            Console.WriteLine("\n\n---------------MCTS MOVED HERE------------------------\n\n");
            return -1;
        }
    }
}

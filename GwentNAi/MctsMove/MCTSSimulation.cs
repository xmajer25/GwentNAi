using GwentNAi.GameSource.Board;
using GwentNAi.MctsMove.Enums;


namespace GwentNAi.MctsMove
{
    public static class MCTSSimulation
    {
        public static Winner Simulation(MCTSNode node)
        {
            MCTSNode clonedNode = (MCTSNode)node.Clone();
            while (!clonedNode.IsTerminal)
            {
                OnPlayersPass(clonedNode);

                if (clonedNode.IsTerminal) break;

                clonedNode.Board.TurnUpdate();
                clonedNode.Board.CurrentPlayerActions.GetAllActions(clonedNode.Board.GetCurrentBoard(), clonedNode.Board.GetCurrentLeader().HandDeck, clonedNode.Board.GetCurrentLeader());
                MCTSRandomMove.PlayRandomEnemieMove(clonedNode);

                clonedNode.Board.TurnUpdate();
                clonedNode.Board.CurrentPlayerActions.GetAllActions(clonedNode.Board.GetCurrentBoard(), clonedNode.Board.GetCurrentLeader().HandDeck, clonedNode.Board.GetCurrentLeader());
                MCTSRandomMove.PlayRandomMove(clonedNode);
            }

            if (clonedNode.Board.Leader1.Victories == 2 && clonedNode.Board.Leader2.Victories == 2) return Winner.Tie;
            if (clonedNode.Board.Leader1.Victories == 2) return Winner.Leader1;
            return Winner.Leader2;
        }

        public static Winner OurTurnSimulation(MCTSNode node)
        {
            MCTSNode clonedNode = (MCTSNode)node.Clone();
            while (!clonedNode.IsTerminal)
            {
                OnPlayersPass(clonedNode);

                if (clonedNode.IsTerminal) break;

                clonedNode.Board.CurrentPlayerActions.GetAllActions(clonedNode.Board.GetCurrentBoard(), clonedNode.Board.GetCurrentLeader().HandDeck, clonedNode.Board.GetCurrentLeader());
                MCTSRandomMove.PlayRandomMove(clonedNode);

                clonedNode.Board.TurnUpdate();

                clonedNode.Board.CurrentPlayerActions.GetAllActions(clonedNode.Board.GetCurrentBoard(), clonedNode.Board.GetCurrentLeader().HandDeck, clonedNode.Board.GetCurrentLeader());
                MCTSRandomMove.PlayRandomEnemieMove(clonedNode);

                clonedNode.Board.TurnUpdate();
                
            }
          
            if (clonedNode.Board.Leader1.Victories == 2 && clonedNode.Board.Leader2.Victories == 2) return Winner.Tie;
            if (clonedNode.Board.Leader1.Victories == 2) return Winner.Leader1;
            return Winner.Leader2;
        }
     
        private static void OnPlayersPass(MCTSNode node)
        {
            GameBoard board = node.Board;
            if(board.Leader1.HasPassed == true && board.Leader2.HasPassed == true)
            {
                DetermineRoundWinner(board);
                board.DrawBothHands(3);
                board.ResetBoard();
                board.CurrentPlayerActions.SwapCards.StopSwapping = true;
            }
        }

        static private void DetermineRoundWinner(GameBoard board)
        {
            if (board.PointSumP1 == board.PointSumP2)
            {
                board.Leader1.Victories++;
                board.Leader2.Victories++;
            }
            else if (board.PointSumP1 > board.PointSumP2)
            {
                board.Leader1.Victories++;
            }
            else
            {
                board.Leader2.Victories++;
            }
        }
    }
}

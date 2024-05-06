using GwentNAi.GameSource.Board;
using GwentNAi.MctsMove.Enums;


namespace GwentNAi.MctsMove
{
    /*
     * Static class containing methods for MCTS simulation
     */
    public static class MCTSSimulation
    {
        /*
         * Simulates rest of the game randomly
         * Random moves are executed on a clonedNode
         * Returns Enum to determine game winner
         */
        public static Winner Simulation(MCTSNode node)
        {
            MCTSNode clonedNode = (MCTSNode)node.Clone();
            while (!clonedNode.IsTerminal)
            {
                OnPlayersPass(clonedNode);

                if (clonedNode.IsTerminal) break;

                clonedNode.Board.TurnUpdate();
                if (!clonedNode.Board.GetCurrentLeader().HasPassed)
                {
                    clonedNode.Board.CurrentPlayerActions.GetAllActions(clonedNode.Board.GetCurrentBoard(), clonedNode.Board.GetCurrentLeader().Hand, clonedNode.Board.GetCurrentLeader());
                    MCTSRandomMove.PlayRandomEnemieMove(clonedNode);
                }


                clonedNode.Board.TurnUpdate();
                if (!clonedNode.Board.GetCurrentLeader().HasPassed)
                {
                    clonedNode.Board.CurrentPlayerActions.GetAllActions(clonedNode.Board.GetCurrentBoard(), clonedNode.Board.GetCurrentLeader().Hand, clonedNode.Board.GetCurrentLeader());
                    MCTSRandomMove.PlayRandomCard(clonedNode);
                }
            }

            if (clonedNode.Board.Leader1.Victories == 2 && clonedNode.Board.Leader2.Victories == 2) return Winner.Tie;
            if (clonedNode.Board.Leader1.Victories == 2) return Winner.Leader1;
            return Winner.Leader2;
        }

        /*
         * Simulation starting with our turn
         * (special case after swapping -> no need to swap enemie)
         * Random moves are executed on a clonedNode
         * Returns Enum to determine winner
         */
        public static Winner OurTurnSimulation(MCTSNode node)
        {
            MCTSNode clonedNode = (MCTSNode)node.Clone();
            while (!clonedNode.IsTerminal)
            {
                OnPlayersPass(clonedNode);

                if (clonedNode.IsTerminal) break;

                if (!clonedNode.Board.GetCurrentLeader().HasPassed)
                {
                    clonedNode.Board.CurrentPlayerActions.GetAllActions(clonedNode.Board.GetCurrentBoard(), clonedNode.Board.GetCurrentLeader().Hand, clonedNode.Board.GetCurrentLeader());
                    MCTSRandomMove.PlayRandomCard(clonedNode);
                }

                clonedNode.Board.TurnUpdate();

                if (!clonedNode.Board.GetCurrentLeader().HasPassed)
                {
                    clonedNode.Board.CurrentPlayerActions.GetAllActions(clonedNode.Board.GetCurrentBoard(), clonedNode.Board.GetCurrentLeader().Hand, clonedNode.Board.GetCurrentLeader());
                    MCTSRandomMove.PlayRandomEnemieMove(clonedNode);
                }

                clonedNode.Board.TurnUpdate();

            }

            if (clonedNode.Board.Leader1.Victories == 2 && clonedNode.Board.Leader2.Victories == 2) return Winner.Tie;
            if (clonedNode.Board.Leader1.Victories == 2) return Winner.Leader1;
            return Winner.Leader2;
        }

        /*
         * Resets board if both players have passed during the simulation
         */
        private static void OnPlayersPass(MCTSNode node)
        {
            GameBoard board = node.Board;
            if (board.Leader1.HasPassed == true && board.Leader2.HasPassed == true)
            {
                DetermineRoundWinner(board);
                board.DrawBothHands(3);
                board.ResetBoard();
                board.CurrentPlayerActions.CardSwaps.StopSwapping = true;
            }
        }

        /*
         * Called after both players passed during simulation
         * Increments victories counter
         */
        static public void DetermineRoundWinner(GameBoard board)
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

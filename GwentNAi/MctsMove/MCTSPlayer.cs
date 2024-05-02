using GwentNAi.GameSource.AssistantClasses;
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.CustomExceptions;
using GwentNAi.GameSource.Player;
using GwentNAi.MctsMove.Enums;
using System.Diagnostics;

namespace GwentNAi.MctsMove
{
    public static class MCTSPlayer
    {
        private static readonly Stopwatch stopwatch = new();

        public static int MCTSMove(GameBoard board)
        {
            stopwatch.Start();

            int Iterations = board.GetCurrentLeader().Iterations;
            int Simulations = board.GetCurrentLeader().Simulations;

            GameBoard clonedBoard = (GameBoard)board.Clone();
            MCTSNode Root = new(null, clonedBoard);
            Winner winner;
            double reward;

            for (int i = 0; i < Iterations; i++)
            {
                //SELECTION
                MCTSNode SelectedNode = Root.Selection(Root.NumberOfVisits);

                if (PlayersHavePassed(SelectedNode))
                {
                    MCTSNode newRoundBoard = (MCTSNode)SelectedNode.Clone();
                    MCTSSimulation.DetermineRoundWinner(newRoundBoard.Board);
                    newRoundBoard.Board.DrawBothHands(3);
                    newRoundBoard.Board.ResetBoard();
                    newRoundBoard.Board.GetSwapCards();

                    SelectedNode.Children.Clear();
                    SelectedNode.AppendChild(newRoundBoard.Board, false, "New round board");
                    SelectedNode = SelectedNode.Children.First();
                }

                //Special case - need to swap cards
                if (SelectedNode.Board.CurrentPlayerActions.CardSwaps.SwapAvailable || PlayerHasSwapped3Cards(SelectedNode))
                {
                    //EXPANSION
                    if (SelectedNode.IsLeaf)
                        MCTSExpandChildren.SwapCard(SelectedNode);
                    SelectedNode = SelectedNode.FirstUnexploredChild();

                    //SIMULATION
                    reward = 0;

                    for(int sim = 0; sim < Simulations; sim++)
                    {
                        if (SelectedNode.EndMove && SelectedNode.Board.CurrentPlayerActions.CardSwaps.PlayersToSwap == 1)
                            winner = MCTSSimulation.OurTurnSimulation(SelectedNode);
                        else
                            winner = MCTSSimulation.Simulation(SelectedNode);

                        reward += DetermineReward(Root, winner);
                    }
                    

                    //BACK_PROPAGATION
                    SelectedNode.UpdateStats(reward);

                    continue;
                }

                //EXPANSION
                if (SelectedNode.IsLeaf && !SelectedNode.Board.GetCurrentLeader().HasPassed)
                {
                    MCTSExpandChildren.PlayCard(SelectedNode);
                    MCTSExpandChildren.OrderCard(SelectedNode);
                    MCTSExpandChildren.LeaderAbility(SelectedNode);
                    MCTSExpandChildren.PassOrEndTurn(SelectedNode);
                }

                SelectedNode = SelectedNode.FirstUnexploredChild();

                //ENEMIE MOVE
                if (SelectedNode.EndMove && !SelectedNode.Board.GetEnemieLeader().HasPassed)
                {
                    MCTSExpandChildren.ExpandOneEnemie(SelectedNode);
                    if (SelectedNode.Children.Count > 0)
                        SelectedNode = SelectedNode.Children.First();
                }

                //SIMULATION
                reward = 0;
                for(int sim = 0; sim < Simulations; sim++) 
                {
                    winner = MCTSSimulation.Simulation(SelectedNode);
                    reward += DetermineReward(Root, winner);
                }
         

                //BACK_PROPAGATION
                SelectedNode.UpdateStats(reward);
            }
            stopwatch.Stop();
            Logging.LogTimeSpent(stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();

            //MODIFY BOARD WITH BEST MOVE AND RETURN
            board = Execute(Root, board);
            return -1;
        }

        private static bool PlayersHavePassed(MCTSNode node)
         => (node.Board.Leader1.HasPassed && node.Board.Leader2.HasPassed);


        private static bool PlayerHasSwapped3Cards(MCTSNode node)
        {
            if (node.Board.CurrentPlayerActions.CardSwaps.CardSwaps != 3) return false;
            for (int i = 1; i <= 3; i++)
            {
                node = node.Parent;
                if (node == null) return false;
                if (node.Board.CurrentPlayerActions.CardSwaps.CardSwaps != i) return false;
            }
            return true;
        }



        private static double DetermineReward(MCTSNode rootNode, Winner winner)
        {
            if (winner == Winner.Tie)
                return (-0.2);

            DefaultLeader victoriousLeader = GetWinner(winner, rootNode.Board);
            return (rootNode.Board.GetCurrentLeader() == victoriousLeader ? 1 : (-1));
        }

        private static DefaultLeader GetWinner(Winner winner, GameBoard board)
            => winner == Winner.Leader1 ? board.Leader1 : board.Leader2;


        private static GameBoard Execute(MCTSNode node, GameBoard board)
        {
            int totalNumberOfVisits = node.NumberOfVisits;
            MCTSNode controlNode = node;
            while (!node.EndMove)
            {
                node = node.BestChild(totalNumberOfVisits);
                Logging.LogMove(node.Board, node.Move);
                if (controlNode == node) break;
                controlNode = node;
            }

            board.Leader1 = (DefaultLeader)node.Board.Leader1.Clone();
            board.Leader2 = (DefaultLeader)node.Board.Leader2.Clone();
            board.PointSumP1 = node.Board.PointSumP1;
            board.PointSumP2 = node.Board.PointSumP2;
            board.CurrentlyPlayingLeader = node.Board.GetCurrentLeader() == node.Board.Leader1 ? board.Leader1 : board.Leader2;
            board.CurrentPlayerBoard = node.Board.GetCurrentBoard() == node.Board.Leader1.Board ? board.Leader1.Board : board.Leader2.Board;
            board.CurrentPlayerActions.ClearImidiateActions();
            if (board.CurrentPlayerActions.CardSwaps.PlayersToSwap > 0)
                board.CurrentPlayerActions.CardSwaps.StopSwapping = true;
            else
                board.CurrentPlayerActions.CardSwaps.Indexes.Clear();

            return board;
        }
    }
}

using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Player;
using GwentNAi.MctsMove.Enums;
using System.Transactions;

namespace GwentNAi.MctsMove
{
    public static class MCTSPlayer
    {
        public static int MCTSMove(GameBoard board)
        {
            GameBoard clonedBoard = (GameBoard)board.Clone();   
            MCTSNode Root = new MCTSNode(null, clonedBoard);
            Winner winner;
            double reward;

            for (int i = 0; i < 1800; i++)
            {
                //SELECTION
                MCTSNode SelectedNode = Root.Selection(Root.NumberOfVisits);

                //Special case - need to swap cards
                if (PlayersHavePassed(SelectedNode) || SelectedNode.Board.CurrentPlayerActions.SwapCards.SwapAvailable || PlayerHasSwapped3Cards(SelectedNode))
                {
                    if (i != 0) SelectedNode.Board.DrawBothHands(3);
                    //EXPANSION
                    if((SelectedNode == Root || SelectedNode.Parent.AllChildrenExplored) && SelectedNode.IsLeaf)
                        MCTSExpandChildren.SwapCard(SelectedNode);
                    SelectedNode = SelectedNode.FirstUnexploredChild();

                    //SIMULATION
                    if (SelectedNode.EndMove && SelectedNode.Board.CurrentPlayerActions.SwapCards.PlayersToSwap == 1)
                        winner = MCTSSimulation.OurTurnSimulation(SelectedNode);
                    else
                        winner = MCTSSimulation.Simulation(SelectedNode);

                    //BACK_PROPAGATION
                    reward = DetermineReward(Root, winner);
                    SelectedNode.UpdateStats(reward);

                    continue;
                }

                //ALSO HANDLE IMIDIATE ACTIONS -> i go sleep now
                //ALSO FIX -> the card that gets points if dominant just goes insane

                //EXPANSION
                if((SelectedNode == Root || SelectedNode.Parent.AllChildrenExplored) && SelectedNode.IsLeaf)
                    MCTSExpandChildren.PlayCard(SelectedNode);
                SelectedNode = SelectedNode.FirstUnexploredChild();

                //SIMULATION
                if (SelectedNode.EndMove)
                    //here should probubly be enemie move -> iam kinda tired so no more explanations, gl
                    winner = MCTSSimulation.Simulation(SelectedNode);
                else
                    winner = MCTSSimulation.OurTurnSimulation(SelectedNode);

                //BACK_PROPAGATION
                reward = DetermineReward(Root, winner);
                SelectedNode.UpdateStats(reward);
            }

            //MODIFY BOARD WITH BEST MOVE AND RETURN
            board = Execute(Root, board);
            return -1;
        }

        private static bool PlayersHavePassed(MCTSNode node)
        {
            if(node.Board.Leader1.HasPassed && node.Board.Leader2.HasPassed)
            {
                node.Board.SwapCards();
                return true;
            }
            return false;
        }

        private static bool PlayerHasSwapped3Cards(MCTSNode node)
        {
            if (node.Board.CurrentPlayerActions.SwapCards.CardSwaps != 3) return false;
            for(int i = 1; i <= 3; i++) 
            {
                node = node.Parent;
                if (node == null) return false;
                if (node.Board.CurrentPlayerActions.SwapCards.CardSwaps != i) return false;
            }
            return true;
        }

        

        private static double DetermineReward(MCTSNode rootNode, Winner winner)
        {
            if(winner == Winner.Tie)
                return -0.2;

            DefaultLeader victoriousLeader = GetWinner(winner, rootNode.Board);
            return (rootNode.Board.CurrentlyPlayingLeader == victoriousLeader ? 1 : -1);
        }

        private static DefaultLeader GetWinner(Winner winner, GameBoard board)
        {
            return (winner == Winner.Leader1 ? board.Leader1 : board.Leader2);
        }

        private static GameBoard Execute(MCTSNode node, GameBoard board)
        {
            int totalNumberOfVisits = node.NumberOfVisits;
            while(!node.EndMove)
            {
                node = node.BestChild(totalNumberOfVisits);
            }

            board.Leader1 = (DefaultLeader)node.Board.Leader1.Clone();
            board.Leader2 = (DefaultLeader)node.Board.Leader2.Clone();
            board.PointSumP1 = node.Board.PointSumP1;
            board.PointSumP2 = node.Board.PointSumP2;
            board.CurrentlyPlayingLeader = node.Board.CurrentlyPlayingLeader;
            board.CurrentPlayerBoard = node.Board.CurrentPlayerBoard;
            board.CurrentPlayerActions.ClearImidiateActions();
            if (board.CurrentPlayerActions.SwapCards.PlayersToSwap > 0)
                board.CurrentPlayerActions.SwapCards.StopSwapping = true;
            else
                board.CurrentPlayerActions.SwapCards.Indexes.Clear();

            return board;
        }
    }
}

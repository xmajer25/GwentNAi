using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Player;
using GwentNAi.MctsMove.Enums;
using System.Xml.Linq;

namespace GwentNAi.MctsMove
{
    public static class MCTSPlayer
    {
        public static int MCTSMove(GameBoard board)
        {
            GameBoard clonedBoard = (GameBoard)board.Clone();   
            MCTSNode Root = new MCTSNode(null, clonedBoard);

            for(int i = 0; i < 100; i++)
            {
                //SELECT
                MCTSNode SelectedNode = Root.Selection(Root.NumberOfVisits);

                //(swap cards case)
                if (PlayersHavePassed(SelectedNode) || SelectedNode.Board.CurrentPlayerActions.SwapCards.SwapAvailable)
                {
                    if (i != 0) SelectedNode.Board.DrawBothHands(3);

                    MCTSExpandChildren.SwapCard(SelectedNode);
                    SelectedNode = SelectedNode.FirstChild();
                    Winner winner;

                    if (SelectedNode.EndMove && SelectedNode.Board.CurrentPlayerActions.SwapCards.PlayersToSwap == 1)
                    {
                        winner = MCTSSimulation.OurTurnSimulation(SelectedNode);
                    }
                    else
                    {
                        winner = MCTSSimulation.Simulation(SelectedNode);
                    }

                    double reward = DetermineReward(Root, winner);
                    SelectedNode.UpdateStats(reward);
                    continue;
                }
                //ALSO HANDLE IMIDIATE ACTIONS -> i go sleep now
                //Expand node
                //IF end turn -> expand random enemie move (or multiple until move ends -> we want to get back on our turn)
                //Simulation
                //BackPropagation
            }
            board.CurrentPlayerActions.ClearImidiateActions();
            board.CurrentPlayerActions.SwapCards.StopSwapping = true;
            //board = Execute(Root);
            return -1;
        }

        private static bool PlayersHavePassed(MCTSNode node)
        {
            if(node.Board.Leader1.hasPassed && node.Board.Leader2.hasPassed)
            {
                node.Board.SwapCards();
                return true;
            }
            return false;
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

        private static GameBoard Execute(MCTSNode node)
        {
            MCTSNode root = node;
            while(!node.EndMove)
            {
                node = node.BestChild(root.NumberOfVisits);
            }
            return (GameBoard)node.Board.Clone();
        }
    }
}

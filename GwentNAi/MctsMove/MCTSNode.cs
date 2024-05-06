using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.CardRepository;

namespace GwentNAi.MctsMove
{
    /*
     * Class representing a node inside MCTS tree
     */
    public class MCTSNode : ICloneable
    {
        public MCTSNode? Parent { get; set; }
        public List<MCTSNode> Children { get; set; }
        public bool EndMove { get; set; }
        public string? Move { get; set; }
        public GameBoard Board { get; set; }
        public int NumberOfVisits { get; set; }
        private double Reward { get; set; }
        public bool IsLeaf => Children == null || Children.Count == 0;
        public bool AllChildrenExplored => Children.All(child => child.NumberOfVisits > 0); // Returns true if each child has been visited at least once
        public bool IsTerminal => Board.Leader1.Victories == 2 || Board.Leader2.Victories == 2; //Returns true if game doesn't continue further
        public MonsterCards EnemieCards { get; set; }

        /*
         */
        public MCTSNode(MCTSNode? parent, GameBoard board)
        {
            Parent = parent;
            Children = new List<MCTSNode>();
            EndMove = false;
            Board = board;
            NumberOfVisits = 0;
            Reward = 0;
            EnemieCards = new MonsterCards();
        }

        /*
         * Creates a deep clone of this node
         */
        public object Clone()
        {
            MCTSNode clonedNode = new(this.Parent, this.Board)
            {
                Board = (GameBoard)Board.Clone(),
                Children = new List<MCTSNode>(Children),
                EndMove = EndMove,
                Move = Move,
                NumberOfVisits = NumberOfVisits,
                Reward = Reward,
                EnemieCards = (MonsterCards)EnemieCards.Clone(),
            };

            return clonedNode;
        }

        /*
         * Recursive method for back-propagation
         */
        public void UpdateStats(double reward)
        {
            NumberOfVisits++;
            Reward += reward;
            if (Parent != null) Parent.UpdateStats(reward);
        }

        /*
         * Method for appending a child into the children list
         */
        public void AppendChild(GameBoard board, bool endMove, string move)
        {
            MCTSNode child = new(this, board);
            child.EndMove = endMove;
            child.Move = move;
            Children.Add(child);
        }

        /*
         * Traverse the tree to the bottom 
         * Return node according to exploration-exploitation
         */
        public MCTSNode Selection(int TotalVisits)
        {
            MCTSNode selectedNode = BestChild(TotalVisits);
            if (selectedNode.IsLeaf) return selectedNode;

            while (selectedNode.AllChildrenExplored)
            {
                selectedNode = selectedNode.BestChild(TotalVisits);
                if (selectedNode.IsLeaf) return selectedNode;
            }
            return selectedNode;
        }

        /*
         * Returns either unvisited node or the node with highest UCB value from the children list
         */
        public MCTSNode BestChild(int TotalVisits)
        {
            if (IsLeaf) return this;

            var unvisitedChild = Children.FirstOrDefault(obj => obj.NumberOfVisits == 0);
            if (unvisitedChild != null)
            {
                return unvisitedChild;
            }

            return Children.OrderByDescending(obj => obj.GetUCB1Value(TotalVisits)).FirstOrDefault();
        }

        /*
         * From children list, returns first unexplored one
         */
        public MCTSNode FirstUnexploredChild()
        {
            if (IsLeaf) return this;
            return Children.FirstOrDefault(obj => obj.NumberOfVisits == 0);
        }

        /*
         * Returns UCB1 value
         * or Max value if node was unvisited
         */
        private double GetUCB1Value(int TotalVisits)
        {
            if (NumberOfVisits == 0)
            {
                return double.MaxValue;
            }

            double averageReward = Reward / NumberOfVisits;
            double explorationTerm = Math.Sqrt(Math.Log(TotalVisits) / NumberOfVisits);
            return averageReward + explorationTerm;
        }
    }
}

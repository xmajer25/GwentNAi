using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.MctsMove
{
    public class MCTSNode : ICloneable
    {
        public MCTSNode? Parent { get; set; }
        public List<MCTSNode> Children { get; set; }
        public bool EndMove { get; set; }
        public string? Move { get; set; }
        public GameBoard Board { get; set; }
        public int NumberOfVisits { get; set; }
        public double Reward { get; set; }
        public bool IsLeaf => Children == null || Children.Count == 0;
        public bool IsTerminal => Board.Leader1.victories == 2 || Board.Leader2.victories == 2;

        public MCTSNode(MCTSNode? parent, GameBoard board) 
        {
            Parent = parent;
            Children = new List<MCTSNode>();
            EndMove = false;
            Board = board;
            NumberOfVisits = 0;
            Reward = 0;
        }

        public object Clone()
        {
            MCTSNode clonedNode = new(this.Parent, this.Board)
            {
                Board = (GameBoard)Board.Clone(),
                Children = new List<MCTSNode>(Children),
                EndMove = EndMove,
                Move = Move,
                NumberOfVisits = NumberOfVisits,
                Reward = Reward
            };

            return clonedNode;
        }            

        public void UpdateStats(double reward)
        {
            NumberOfVisits++;
            Reward += reward;
            if (Parent != null) Parent.UpdateStats(reward);
        }

        public void AppendChild(GameBoard board, bool endMove)
        {
            MCTSNode child = new(this, board);
            child.EndMove = endMove;
            Children.Add(child);
        }

        public MCTSNode Selection(int TotalVisits)
        {
            MCTSNode selectedNode =  BestChild(TotalVisits);
            while(!selectedNode.IsLeaf)
            {
                selectedNode = selectedNode.BestChild(TotalVisits);
            }
            return selectedNode;
        }

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

        public MCTSNode FirstChild()
        {
            if (IsLeaf) return this;
            return Children[0];
        }

        private double GetUCB1Value(int TotalVisits)
        {
            if(NumberOfVisits == 0)
            {
                return double.MaxValue;
            }

            double averageReward = Reward / NumberOfVisits;
            double explorationTerm = Math.Sqrt(Math.Log(TotalVisits) / averageReward);
            return averageReward + explorationTerm;
        }
    }
}

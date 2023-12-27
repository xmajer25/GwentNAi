using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.CardRepository;
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
        public bool AllChildrenExplored => Children.All(child => child.NumberOfVisits > 0);
        public bool IsTerminal => Board.Leader1.Victories == 2 || Board.Leader2.Victories == 2;
        public MonsterCards EnemieCards { get; set; }

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

        public void AppendChild(GameBoard board, bool endMove, string move)
        {
            MCTSNode child = new(this, board);
            child.EndMove = endMove;
            child.Move = move;
            Children.Add(child);
        }

        public MCTSNode Selection(int TotalVisits)
        {
            MCTSNode selectedNode =  BestChild(TotalVisits);
            if (selectedNode.IsLeaf) return selectedNode;

            while(selectedNode.AllChildrenExplored)
            {
                selectedNode = selectedNode.BestChild(TotalVisits);
                if(selectedNode.IsLeaf) return selectedNode;
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

        public MCTSNode FirstUnexploredChild()
        {
            if (IsLeaf) return this;
            return Children.FirstOrDefault(obj => obj.NumberOfVisits == 0);
        }

        private double GetUCB1Value(int TotalVisits)
        {
            if(NumberOfVisits == 0)
            {
                return double.MaxValue;
            }

            double averageReward = Reward / NumberOfVisits;
            double explorationTerm = Math.Sqrt(Math.Log(TotalVisits) / NumberOfVisits);
            return averageReward + explorationTerm;
        }
    }
}

using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.MctsMove
{
    public class MCTSNode
    {
        public MCTSNode? Parent { get; set; }
        public List<MCTSNode> Children { get; set; }
        public string? Move { get; set; }
        public GameBoard Board { get; set; }
        public int NumberOfVisits { get; set; }
        public float Reward { get; set; }

        public float AverageReward => Reward / NumberOfVisits;
        public bool IsLeaf => Children == null || Children.Count == 0;

        public MCTSNode(MCTSNode? parent, GameBoard board) 
        {
            Parent = parent;
            Children = new List<MCTSNode>();
            Board = board;
            NumberOfVisits = 0;
            Reward = 0;
        }

        public void UpdateStats(float reward)
        {
            NumberOfVisits++;
            Reward += reward;
            if(Parent != null) Parent.UpdateStats(reward);
        }

        public void AppendChild(GameBoard board)
        {
            MCTSNode child = new(this, board);
            Children.Add(child);
        }

        public MCTSNode? BestChild()
        {
            if (Children == null || Children.Count == 0) return null;

            var unvisitedChild = Children.FirstOrDefault(obj => obj.NumberOfVisits == 0);
            if (unvisitedChild != null)
            {
                return unvisitedChild;
            }

            return Children.OrderByDescending(obj => obj.AverageReward).FirstOrDefault();
        }
    }
}

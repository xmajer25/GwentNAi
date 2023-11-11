using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class WildHuntRider : DefaultCard, IDeploy
    {
        public WildHuntRider()
        {
            currentValue = 4;
            maxValue = 4;
            shield = 0;
            provisionValue = 5;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "WildHuntRider";
            shortName = "WH.Rider";
            descriptors = new List<string>() { "Elf", "Wild Hunt", "Warrior" };
            timeToOrder = -1;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            if (!IsDominant(board)) return;

            int thisIndex = board.CurrentPlayerBoard[0].IndexOf((DefaultCard)this);
            int thisRow = 0;
            int numberOfCopies = GetNumberOfCopies(board);
            if (thisIndex == -1)
            {
                thisIndex = board.CurrentPlayerBoard[1].IndexOf((DefaultCard)this);
                thisRow = 1;
            }
            
            for(int i = 0; i < numberOfCopies; i++)
            {
                if (board.CurrentPlayerBoard[thisRow].Count != 9)
                {
                    DefaultCard insertedCard = new WildHuntRider();
                    board.CurrentPlayerBoard[thisRow].Insert(thisIndex + 1 + i, insertedCard);
                }
            }
        }

        private int GetNumberOfCopies(GameBoard board)
        {
            int count = board.CurrentlyPlayingLeader.startingDeck.Cards.OfType<WildHuntRider>().Count();
            board.CurrentlyPlayingLeader.startingDeck.Cards.RemoveAll(obj => obj is WildHuntRider);

            return count;
        }

        private bool IsDominant(GameBoard board)
        {
            List<List<DefaultCard>> enemiePlayerBoard = (board.CurrentPlayerBoard == board.Leader1.Board ? board.Leader2.Board : board.Leader1.Board);
            DefaultCard currentMax = board.CurrentPlayerBoard
                .SelectMany(list => list)
                .OrderByDescending(obj => obj.currentValue)
                .FirstOrDefault();

            DefaultCard enemieMax = enemiePlayerBoard
                .SelectMany(list => list)
                .OrderByDescending(obj => obj.currentValue)
                .FirstOrDefault();

            if (enemieMax == null) return true;
            if(currentMax != null)
            {
                return currentMax.currentValue >= enemieMax.currentValue;
            }
            return true;
        }
    }
}

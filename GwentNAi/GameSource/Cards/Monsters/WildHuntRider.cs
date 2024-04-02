using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class WildHuntRider : DefaultCard, IDeploy
    {
        public WildHuntRider()
        {
            CurrentValue = 4;
            MaxValue = 4;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "WildHuntRider";
            ShortName = "WH.Rider";
            Descriptors = new List<string>() { "Elf", "Wild Hunt", "Warrior" };
            TimeToOrder = -1;
            Bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            if (!IsDominant(board)) return;

            int thisIndex = board.GetCurrentBoard()[0].IndexOf((DefaultCard)this);
            int thisRow = 0;
            int numberOfCopies = GetNumberOfCopies(board);
            if (thisIndex == -1)
            {
                thisIndex = board.GetCurrentBoard()[1].IndexOf((DefaultCard)this);
                thisRow = 1;
            }

            for (int i = 0; i < numberOfCopies; i++)
            {
                if (board.GetCurrentBoard()[thisRow].Count != 9)
                {
                    DefaultCard insertedCard = new WildHuntRider();
                    board.GetCurrentBoard()[thisRow].Insert(thisIndex + 1 + i, insertedCard);
                }
            }
        }

        private int GetNumberOfCopies(GameBoard board)
        {
            int count = board.GetCurrentLeader().StartingDeck.Cards.OfType<WildHuntRider>().Count();
            board.GetCurrentLeader().StartingDeck.Cards.RemoveAll(obj => obj is WildHuntRider);

            return count;
        }

        private bool IsDominant(GameBoard board)
        {
            List<List<DefaultCard>> enemiePlayerBoard = board.GetEnemieBoard();
            DefaultCard currentMax = board.GetCurrentBoard()
                .SelectMany(list => list)
                .OrderByDescending(obj => obj.CurrentValue)
                .FirstOrDefault();

            DefaultCard enemieMax = enemiePlayerBoard
                .SelectMany(list => list)
                .OrderByDescending(obj => obj.CurrentValue)
                .FirstOrDefault();

            if (enemieMax == null) return true;
            if (currentMax != null)
            {
                return currentMax.CurrentValue >= enemieMax.CurrentValue;
            }
            return true;
        }
    }
}

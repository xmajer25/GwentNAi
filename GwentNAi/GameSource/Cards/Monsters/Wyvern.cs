using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Wyvern : DefaultCard, IDeploy, IDeployExpandPickEnemies, IThrive
    {
        public Wyvern()
        {
            CurrentValue = 3;
            MaxValue = 3;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Wyvern";
            ShortName = "Wyvern";
            Descriptors = new List<string>() { "Beast" };
            TimeToOrder = -1;
            Bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();

            for (int currentRow = 0; currentRow < enemieBoard.Count; currentRow++)
            {
                for (int currentIndex = 0; currentIndex < enemieBoard[currentRow].Count; currentIndex++)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentIndex);
                }
            }
        }

        public void postPickEnemieAbilitiy(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            enemieBoard[row][index].TakeDemage(2, false, board);
        }

        public void Thrive(int playedUnitValue)
        {
            if (playedUnitValue > CurrentValue)
            {
                CurrentValue++;
                if (CurrentValue > MaxValue) MaxValue++;

            }
        }
    }
}

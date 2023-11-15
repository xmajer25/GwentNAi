using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Neutral
{
    public class GeraltProfessional : DefaultCard, IUpdate, IOrder, IOrderExpandPickEnemie
    {
        public GeraltProfessional()
        {
            currentValue = 3;
            maxValue = 3;
            shield = 0;
            provisionValue = 11;
            border = 1;
            type = "unit";
            faction = "neutral";
            name = "GeraltProfessional";
            shortName = "Geralt:P";
            descriptors = new List<string>() { "Witcher" };
            timeToOrder = 1;
            bleeding = 0;
        }

        public void Order(GameBoard board)
        {
            pickEnemie(board);
        }

        public void pickEnemie(GameBoard board)
        {
            List<List<int>> enemieIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            List<List<DefaultCard>> enemieBoard;
            enemieBoard = (board.CurrentPlayerBoard == board.Leader1.Board ? board.Leader2.Board : board.Leader1.Board);
            int currentRow = 0;
            int currentIndex = 0;

            foreach(var row in enemieBoard)
            {
                foreach(var card in row)
                {
                    enemieIndexes[currentRow].Add(currentIndex);
                    currentIndex++;
                }
                currentIndex = 0;
                currentRow++;
            }

            board.CurrentPlayerActions.ImidiateActions[0] = enemieIndexes;
        }

        public void postPickEnemieOrder(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = (board.CurrentPlayerBoard == board.Leader1.Board ? board.Leader2.Board : board.Leader1.Board);
            if (enemieBoard[row][index].currentValue % 3 == 0) enemieBoard[row][index].TakeDemage(enemieBoard[row][index].currentValue, false, board);
            else enemieBoard[row][index].TakeDemage(3, false, board);
            timeToOrder--;
            board.CurrentlyPlayingLeader.UseAbility();
        }

        public void StartTurnUpdate()
        {
            if (timeToOrder > 0)
            {
                timeToOrder--;
            }
        }
    }
}

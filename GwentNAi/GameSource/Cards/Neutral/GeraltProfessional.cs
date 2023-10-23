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
            provisionValue = 11;
            border = 1;
            type = "unit";
            faction = "neutral";
            name = "GeraltProfessional";
            shortName = "Geralt:P";
            descriptors = new List<string>() { "witcher" };
            timeToOrder = 1;
        }

        void IOrder.Order(GameBoard board)
        {
            pickEnemie(this, board);
        }

        void IOrderExpandPickEnemie.pickEnemie(GameBoard board)
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

            board.CurrentPlayerActions.ImidiateActions = enemieIndexes;
        }

        void IOrderExpandPickEnemie.postPickEnemieOrder(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = (board.CurrentPlayerBoard == board.Leader1.Board ? board.Leader2.Board : board.Leader1.Board);
            if (enemieBoard[row][index].currentValue % 3 == 0) enemieBoard[row][index].currentValue = 0;
            else enemieBoard[row][index].currentValue -= 3;
            timeToOrder--;
            board.CurrentlyPlayingLeader.UseAbility();
        }

        void IUpdate.StartTurnUpdate()
        {
            if (timeToOrder > 0)
            {
                timeToOrder--;
            }
        }
    }
}

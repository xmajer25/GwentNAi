using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Neutral
{
    public class GeraltProfessional : DefaultCard, IUpdate, IOrder, IOrderExpandPickEnemie
    {
        public GeraltProfessional()
        {
            pointValue = 3;
            provisionValue = 11;
            border = 1;
            type = "unit";
            faction = "neutral";
            name = "GeraltProfessional";
            descriptors = new List<string>() { "witcher" };
            timeToOrder = 1;
        }

        void IOrder.Order(GameBoard board)
        {
            Console.WriteLine("Geralt: Professional Order");
            timeToOrder--;
            pickEnemie(this, board);
        }

        //gotta do the postExpansion shit thing function thinking of killing my self daily
        //and than add that shit in the HUMAN MOVE thing so it all works nicely :)))
        //and dont forget!! skibidi toilet is watching
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
                currentRow++;
            }

            board.CurrentPlayerActions.ImidiateActions = enemieIndexes;
        }

        void IOrderExpandPickEnemie.postPickEnemieOrder(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = (board.CurrentPlayerBoard == board.Leader1.Board ? board.Leader2.Board : board.Leader1.Board);
            if (enemieBoard[row][index].pointValue % 3 == 0) enemieBoard[row].RemoveAt(index);
            else enemieBoard[row][index].pointValue -= 3;
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

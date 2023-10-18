
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards
{
    public class DefaultCard : IPlayCardExpand
    {
        public int pointValue { get; set; }
        public int provisionValue { get; set; }
        public int border { get; set; }
        public string type { get; set; } = string.Empty;
        public string faction { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string shortName { get; set; } = string.Empty;
        public List<string> descriptors { get; set; }  = new List<string>();

        public int timeToOrder { get; set; }

        public void Order(IOrder obj, GameBoard board)
        {
            obj.Order(board);
        }

        public void Cooldown(ICooldown obj, int cooldown)
        {
            obj.Cooldown(cooldown);
        }

        public void StartTurnUpdate(IUpdate obj)
        {
            obj.StartTurnUpdate();
        }

        public void pickEnemie(IOrderExpandPickEnemie obj, GameBoard board)
        {
            obj.pickEnemie(board);
        }

        public void postPickEnemieOrder(IOrderExpandPickEnemie obj, GameBoard board, int row, int index)
        {
            obj.postPickEnemieOrder(board, row, index);
        }

        public virtual void PlayCardExpand(GameBoard board)
        {
            List<List<DefaultCard>> CPboard = board.CurrentlyPlayingLeader.Board;
            List<List<int>> possibleIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            int currentRow = 0;
            int currentCulumn = 0;

            foreach (var row in CPboard)
            {
                foreach (var card in row)
                {
                    possibleIndexes[currentRow].Add(currentCulumn);
                    currentCulumn++;
                }
                possibleIndexes[currentRow].Add(currentCulumn);
                if (possibleIndexes[currentRow].Count == 10) possibleIndexes[currentRow].Clear();
                currentRow++;
                currentCulumn = 0;
            }

            board.CurrentPlayerActions.ImidiateActions = possibleIndexes;
        }
    }
}

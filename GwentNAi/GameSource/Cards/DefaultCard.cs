
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards
{
    public class DefaultCard : IPlaySelfExpand
    {
        public int currentValue { get; set; }
        public int maxValue { get; set; }
        public int provisionValue { get; set; }
        public int border { get; set; }
        public int bleeding { get; set; }
        public string type { get; set; } = string.Empty;
        public string faction { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string shortName { get; set; } = string.Empty;
        public List<string> descriptors { get; set; } = new List<string>();

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

        public void pickAll(IOrderExpandPickAll obj, GameBoard board)
        {
            obj.pickAll(board);
        }

        public void postPickAllOrder(IOrderExpandPickAll obj, GameBoard board, int player, int row, int index)
        {
            obj.postPickAllOrder(board, player, row, index);
        }

        public virtual void PlaySelfExpand(GameBoard board)
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

            board.CurrentPlayerActions.ImidiateActions[0] = possibleIndexes;
        }

        public void PostPlayCardOrder(IPlayCardExpand obj, GameBoard board, int row, int column)
        {
            obj.PostPlayCardOrder(board, row, column);
        }

        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            throw new NotImplementedException();
        }

        public void RespondToBleeding(IBleedingInteraction obj)
        {
            obj.RespondToBleeding();
        }

        public void Deploy(IDeploy obj, GameBoard board)
        {
            obj.Deploy(board);
        }

        public void postPickEnemieAbilitiy(IDeployExpandPickEnemies obj, GameBoard board, int row, int column)
        {
            obj.postPickEnemieAbilitiy(board, row, column);
        }

        public void postPickCardAbility(IDeployExpandPickCard obj, GameBoard board, int index)
        {
            obj.postPickCardAbility(board, index);
        }

        public void DeathwishAbility(IDeathwish obj, GameBoard board)
        {
            obj.DeathwishAbility(board);
        }

        public void RespondToCrone(ICroneInteraction obj)
        {
            obj.RespondToCrone();
        }
        public void PostPickAllyOrder(IOrderExpandPickAlly obj, GameBoard board, int row, int index)
        {
            obj.PostPickAllyOrder(board, row, index);
        }
    }
}

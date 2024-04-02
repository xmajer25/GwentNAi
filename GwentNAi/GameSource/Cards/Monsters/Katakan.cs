using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Katakan : DefaultCard, IOrder, ICooldown, IPlayCardExpand, IUpdate, IBleedingInteraction
    {
        public Katakan()
        {
            CurrentValue = 5;
            MaxValue = 5;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Katakan";
            ShortName = "Katakan";
            Descriptors = new List<string>() { "Vampire" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        public void Order(GameBoard board)
        {
            if (TimeToOrder != 0) return;
            PlayCardExpand(board);
        }
        public void Cooldown(int cooldown)
        {
            TimeToOrder += (cooldown + 1);
        }

        public void PlayCardExpand(GameBoard board)
        {
            List<List<DefaultCard>> CPboard = board.GetCurrentBoard();
            List<List<int>> possibleIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };

            int currentRow = GetCurrentRow(board);
            int currentCulumn = 0;


            foreach (var card in CPboard[currentRow])
            {
                possibleIndexes[currentRow].Add(currentCulumn);
                currentCulumn++;
            }
            possibleIndexes[currentRow].Add(currentCulumn);
            if (possibleIndexes[currentRow].Count == 10) possibleIndexes[currentRow].Clear();

            board.CurrentPlayerActions.ImidiateActions[0] = possibleIndexes;
        }

        private int GetCurrentRow(GameBoard board)
        {
            int isInRow = board.GetCurrentBoard()[0].IndexOf(this);
            return (isInRow == -1) ? 1 : 0;
        }

        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            DefaultCard playedCard = new Ekimmara();
            board.GetCurrentBoard()[row].Insert(column, playedCard);
            Cooldown(4);
            board.GetCurrentLeader().UseAbility();
        }

        void IUpdate.StartTurnUpdate()
        {
            if (TimeToOrder > 0)
            {
                TimeToOrder--;
            }
        }

        public void RespondToBleeding()
        {
            if (TimeToOrder > 0)
            {
                TimeToOrder--;
            }
        }
    }
}

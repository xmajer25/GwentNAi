using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Katakan : DefaultCard, IOrder, ICooldown, IPlayCardExpand, IUpdate, IBleedingInteraction
    {
        /*
         * Initialize information about specific card 
         */
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

        /*
         * Order if time to order expired
         */
        public void Order(GameBoard board)
        {
            if (TimeToOrder != 0) return;
            PlayCardExpand(board);
        }

        /*
         * Adds cooldown on the ability
         */
        public void Cooldown(int cooldown)
        {
            TimeToOrder += (cooldown + 1);
        }

        /*
         * Fills imidiate actions with possible positions for summoning a card
         */
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

        /*
         * Returns row number of this card
         */
        private int GetCurrentRow(GameBoard board)
        {
            int isInRow = board.GetCurrentBoard()[0].IndexOf(this);
            return (isInRow == -1) ? 1 : 0;
        }

        /*
         * Executes order
         * Summons Ekimmara on row-column
         */
        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            DefaultCard playedCard = new Ekimmara();
            board.GetCurrentBoard()[row].Insert(column, playedCard);
            Katakan card = GetSelf(board);
            if (card != null) card.Cooldown(4);
            board.GetCurrentLeader().UseAbility();
        }

        /*
         * At the start of a turn reduce the time to order
         */
        public void StartTurnUpdate()
        {
            if (TimeToOrder > 0)
            {
                TimeToOrder--;
            }
        }

        /*
         * Whenever a card bleads -> reduce time to order
         */
        public void RespondToBleeding()
        {
            if (TimeToOrder > 0)
            {
                TimeToOrder--;
            }
        }

        /*
         * Returns Katakan if there is one on the current board
         */
        private Katakan GetSelf(GameBoard board)
        {
            foreach(var row in  board.GetCurrentBoard())
            {
                foreach(DefaultCard card in  row)
                {
                    if(card == this) return card as Katakan;
                }
            }
            return null;
        }
    }
}

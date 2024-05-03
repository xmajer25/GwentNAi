
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards
{
    /*
     * Parent class for all of the specific cards
     * Contains few general methods and attributes for defining a card
     */
    public class DefaultCard : IPlaySelfExpand, ICloneable
    {
        public virtual int CurrentValue { get; set; }
        public int MaxValue { get; set; }
        public int Shield { get; set; }
        public int Border { get; set; }
        public int Bleeding { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Faction { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public List<string> Descriptors { get; set; } = new List<string>();

        public int TimeToOrder { get; set; }

        /*
         * Creates a deep clone of a card
         */
        public object Clone()
        {
            DefaultCard clonedInstance = (DefaultCard)base.MemberwiseClone();
            // Deep clone for List<string> properties
            clonedInstance.Descriptors = new List<string>(Descriptors);
            if (clonedInstance.Name != this.Name) throw new Exception("Inner error: incorect name cloned");
            return clonedInstance;
        }

        /*
         * Fills imidiate actions with possible card placements 
         */
        public virtual void GetPlacementOptions(GameBoard board)
        {
            List<List<DefaultCard>> CPboard = board.GetCurrentBoard();
            board.CurrentPlayerActions.ClearImidiateActions();


            for (int row = 0; row < CPboard.Count; row++)
            {
                for (int i = 0; i <= CPboard[row].Count; i++)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][row].Add(i);
                }
                //CLEAR IF ROW IS FULL
                if (board.CurrentPlayerActions.ImidiateActions[0][row].Count == 10) board.CurrentPlayerActions.ImidiateActions[0][row].Clear();
            }

        }

        /*
         * Method for taking damage
         */
        public virtual void TakeDemage(int damage, bool lethal, GameBoard board)
        {
            if (lethal)
            {
                CurrentValue = 0;
                return;
            }
            int _excessDamage = damage - Shield;
            Shield -= damage;
            if (Shield < 0) Shield = 0;
            if (_excessDamage > 0) CurrentValue -= _excessDamage;
        }
    }
}

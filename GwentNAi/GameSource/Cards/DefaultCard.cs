
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards
{
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

        public object Clone()
        {
            DefaultCard clonedInstance = (DefaultCard)base.MemberwiseClone();

            // Deep clone for List<string> properties
            clonedInstance.Descriptors = new List<string>(Descriptors);
            if (clonedInstance.Name != this.Name) throw new Exception("Inner error: incorect name cloned");
            return clonedInstance;
        }

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
                if (board.CurrentPlayerActions.ImidiateActions[0][row].Any(index => index > CPboard[row].Count)) throw new Exception();
                if (board.CurrentPlayerActions.ImidiateActions[0][row].Count - 1 > CPboard[row].Count) throw new Exception("Inner Error: index out of range added");

            }

        }

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

        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            throw new NotImplementedException();
        }
    }
}

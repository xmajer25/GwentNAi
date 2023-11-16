
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using System.Reflection;

namespace GwentNAi.GameSource.Cards
{
    public class DefaultCard : IPlaySelfExpand, ICloneable
    {
        public virtual int currentValue { get; set; }
        public int maxValue { get; set; }
        public int shield { get; set; }
        public int provisionValue { get; set; }
        public int border { get; set; }
        public int bleeding { get; set; }
        public string type { get; set; } = string.Empty;
        public string faction { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string shortName { get; set; } = string.Empty;
        public List<string> descriptors { get; set; } = new List<string>();

        public int timeToOrder { get; set; }

        public object Clone()
        {
            DefaultCard clonedInstance = (DefaultCard)Activator.CreateInstance(this.GetType());
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach(PropertyInfo property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    if (property.PropertyType == typeof(List<string>))
                    {
                        List<string> originalList = (List<string>)property.GetValue(this);
                        List<string> clonedList = originalList.Select(s => string.Copy(s)).ToList();
                        property.SetValue(clonedInstance, clonedList);
                    }
                    else
                    {
                        property.SetValue(clonedInstance, property.GetValue(this));
                    }
                }
            }
            return clonedInstance;
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

        public virtual void TakeDemage(int damage, bool lethal, GameBoard board)
        {
            if (lethal)
            {
                currentValue = 0;
                return;
            }
            int _excessDamage = damage - shield;
            shield -= damage;
            if(shield < 0) shield = 0;
            if(_excessDamage > 0) currentValue -= _excessDamage;
        }

        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            throw new NotImplementedException();
        }
    }
}

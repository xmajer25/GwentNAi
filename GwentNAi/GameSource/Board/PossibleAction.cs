
using GwentNAi.GameSource.Cards;

namespace GwentNAi.GameSource.Board
{
    public class PossibleAction : ICloneable
    {
        public DefaultCard ActionCard { get; set; }
        public string CardName { get; set; } = string.Empty;

        public object Clone()
        {
            return new PossibleAction()
            {
                ActionCard = ActionCard,
                CardName = CardName
            };
        }
    }
}

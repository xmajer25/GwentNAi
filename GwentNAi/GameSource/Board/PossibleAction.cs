
using GwentNAi.GameSource.Cards;

namespace GwentNAi.GameSource.Board
{
    /*
     * Class represents one possible action
     */
    public class PossibleAction : ICloneable
    {
        public DefaultCard ActionCard { get; set; }
        public string CardName { get; set; } = string.Empty;

        /*
         * Creates a deep clone of a possible action
         */
        public object Clone()
        {
            return new PossibleAction()
            {
                ActionCard = (DefaultCard)ActionCard.Clone(),
                CardName = CardName
            };
        }
    }
}

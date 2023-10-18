using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Cards.Neutral
{
    public class OlgierVonEverec : DefaultCard, IOrder, ICooldown, IUpdate
    {
        public OlgierVonEverec()
        {
            pointValue = 8;
            provisionValue = 8;
            border = 1;
            type = "unit";
            faction = "neutral";
            name = "OlgrierdVonEverec";
            shortName = "Olgrierd";
            descriptors = new List<string>() { "human", "cursed", "bandit" };
            timeToOrder = 1;
        }

        void IOrder.Order(GameBoard board)
        {
            Cooldown(this, 1);
           
        }

        void ICooldown.Cooldown(int cooldown)
        {
            timeToOrder += (cooldown + 1);
        }

        void IUpdate.StartTurnUpdate()
        {
            if(timeToOrder > 0)
            {
                timeToOrder--;
            }
        }
    }
}

using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Cards.Neutral
{
    public class OlgierVonEverec : DefaultCard, IOrder, ICooldown, IUpdate
    {
        public OlgierVonEverec()
        {
            currentValue = 8;
            maxValue = 8;
            shield = 0;
            provisionValue = 8;
            border = 1;
            type = "unit";
            faction = "neutral";
            name = "OlgrierdVonEverec";
            shortName = "Olgrierd";
            descriptors = new List<string>() { "Human", "Cursed", "Bandit" };
            timeToOrder = 1;
            bleeding = 0;
        }

        public void Order(GameBoard board)
        {
            Cooldown(1);
            board.CurrentlyPlayingLeader.UseAbility();
            if (currentValue < maxValue) currentValue++;
        }

        public void Cooldown(int cooldown)
        {
            timeToOrder += (cooldown + 1);
        }

        public void StartTurnUpdate()
        {
            if(timeToOrder > 0)
            {
                timeToOrder--;
            }
        }
    }
}

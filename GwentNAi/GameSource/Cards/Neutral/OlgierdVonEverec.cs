using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Neutral
{
    public class OlgierVonEverec : DefaultCard, IOrder, ICooldown, IUpdate
    {
        public OlgierVonEverec()
        {
            CurrentValue = 8;
            MaxValue = 8;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "neutral";
            Name = "OlgrierdVonEverec";
            ShortName = "Olgrierd";
            Descriptors = new List<string>() { "Human", "Cursed", "Bandit" };
            TimeToOrder = 1;
            Bleeding = 0;
        }

        public void Order(GameBoard board)
        {
            Cooldown(1);
            board.CurrentlyPlayingLeader.UseAbility();
            if (CurrentValue < MaxValue) CurrentValue++;
        }

        public void Cooldown(int cooldown)
        {
            TimeToOrder += (cooldown + 1);
        }

        public void StartTurnUpdate()
        {
            if (TimeToOrder > 0)
            {
                TimeToOrder--;
            }
        }
    }
}

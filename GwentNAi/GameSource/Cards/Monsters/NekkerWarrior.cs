using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class NekkerWarrior : DefaultCard, IDeploy, IThrive
    {
        private bool hasTriggeredThrive;
        public NekkerWarrior()
        {
            CurrentValue = 7;
            MaxValue = 7;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "NekkerWarrior";
            ShortName = "Nekker:W";
            Descriptors = new List<string>() { "Ogroid", "Warrior" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            hasTriggeredThrive = board.GetCurrentBoard()[0].Any(obj => obj is IThrive && obj.CurrentValue < CurrentValue);
            if (!hasTriggeredThrive)
            {
                hasTriggeredThrive = board.GetCurrentBoard()[1].Any(obj => obj is IThrive && obj.CurrentValue < CurrentValue);
            }
            if (!hasTriggeredThrive)
            {
                TakeDemage(3, false, board);
            }
        }

        public void Thrive(int playedUnitValue)
        {
            if (!hasTriggeredThrive)
            {
                if (playedUnitValue > CurrentValue)
                {
                    CurrentValue++;
                    if (CurrentValue > MaxValue) MaxValue++;

                }
            }
        }
    }
}

using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Nekker : DefaultCard, IDeploy, IThrive
    {
        public Nekker()
        {
            CurrentValue = 1;
            MaxValue = 1;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Nekker";
            ShortName = "Nekker";
            Descriptors = new List<string>() { "Ogroid" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<DefaultCard>> currentBoard = board.GetCurrentBoard();
            int thisIndex = currentBoard[0].IndexOf((DefaultCard)this);
            int thisRow = 0;
            if (thisIndex == -1)
            {
                thisIndex = currentBoard[1].IndexOf((DefaultCard)this);
                thisRow = 1;
            }

            if (currentBoard[thisRow].Count != 9)
            {
                currentBoard[thisRow].Insert(thisIndex + 1, new Nekker());
            }
        }

        public void Thrive(int playedUnitValue)
        {
            if (playedUnitValue > CurrentValue)
            {
                CurrentValue++;
                if (CurrentValue > MaxValue) MaxValue++;

            }
        }
    }
}

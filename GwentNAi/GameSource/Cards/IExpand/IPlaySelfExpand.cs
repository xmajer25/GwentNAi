using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IPlaySelfExpand
    {
        public void GetPlacementOptions(GameBoard board);
    }
}

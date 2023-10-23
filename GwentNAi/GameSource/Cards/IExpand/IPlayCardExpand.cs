
using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IPlayCardExpand
    {
        public void PlayCardExpand(GameBoard board);

        public void PostPlayCardOrder(GameBoard board, int row, int column);
    }
}

using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IOrderExpandPickAll
    {
        public void pickAll(GameBoard board);
        public void postPickAllOrder(GameBoard board, int player, int row, int index);
    }
}

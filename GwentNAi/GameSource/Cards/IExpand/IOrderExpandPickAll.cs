using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IOrderExpandPickAll
    {
        public void PickAll(GameBoard board);
        public void PostPickAllOrder(GameBoard board, int player, int row, int index);
    }
}

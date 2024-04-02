using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IOrderExpandPickAlly
    {
        void PostPickAllyOrder(GameBoard board, int row, int index);
    }
}

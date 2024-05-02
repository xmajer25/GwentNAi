using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IOrderExpandPickEnemie
    {
        public void PickEnemie(GameBoard board);
        public void PostPickEnemieOrder(GameBoard board, int row, int index);
    }
}

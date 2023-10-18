using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IOrderExpandPickEnemie
    {
        public void pickEnemie(GameBoard board);
        public void postPickEnemieOrder(GameBoard board, int row, int index);
    }
}

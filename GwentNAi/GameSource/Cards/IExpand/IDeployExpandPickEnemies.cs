using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IDeployExpandPickEnemies
    {
        public void postPickEnemieAbilitiy(GameBoard board, int row, int index);
    }
}

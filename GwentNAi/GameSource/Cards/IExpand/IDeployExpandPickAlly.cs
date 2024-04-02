using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IDeployExpandPickAlly
    {
        public void PostPickAllyAbilitiy(GameBoard board, int row, int index);
    }
}

using GwentNAi.GameSource.Board;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IDeployExpandPickCard
    {
        void postPickCardAbility(GameBoard board, int cardIndex);
    }
}

using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IDeployExpandPickCard
    {
        void postPickCardAbility(GameBoard board, int cardIndex);
    }
}

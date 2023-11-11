using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.IDefault
{
    public interface IEndTurnUpdate
    {
        void EndTurnUpdate(GameBoard board);
    }
}

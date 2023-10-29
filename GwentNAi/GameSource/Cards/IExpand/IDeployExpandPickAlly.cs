using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IDeployExpandPickAlly
    {
        public void PostPickAllyAbilitiy(GameBoard board, int row, int index);
    }
}

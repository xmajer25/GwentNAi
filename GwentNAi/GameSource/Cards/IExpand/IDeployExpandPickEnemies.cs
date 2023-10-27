using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IDeployExpandPickEnemies
    {
        public void postPickEnemieAbilitiy(GameBoard board, int row, int index);
    }
}

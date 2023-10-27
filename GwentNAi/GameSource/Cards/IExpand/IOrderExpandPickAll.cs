using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.IExpand
{
    public interface IOrderExpandPickAll
    {
        public void pickAll(GameBoard board);
        public void postPickAllOrder(GameBoard board, int player, int row, int index);
    }
}

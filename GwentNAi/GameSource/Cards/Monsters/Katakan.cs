using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Katakan : DefaultCard, IOrder, ICooldown, IPlayCardExpand, IUpdate, IBleedingInteraction
    {
        public Katakan()
        {
            currentValue = 5;
            maxValue = 5;
            shield = 0;
            provisionValue = 9;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Katakan";
            shortName = "Katakan";
            descriptors = new List<string>() { "Vampire"};
            timeToOrder = 0;
            bleeding = 0;
        }

        void IOrder.Order(GameBoard board)
        {
            PlayCardExpand(board);
        }
        void ICooldown.Cooldown(int cooldown)
        {
            timeToOrder += (cooldown + 1);
        }

        public void PlayCardExpand(GameBoard board)
        {
            List<List<DefaultCard>> CPboard = board.CurrentlyPlayingLeader.Board;
            List<List<int>> possibleIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };

            int currentRow = GetCurrentRow(board);
            int currentCulumn = 0;


            foreach (var card in CPboard[currentRow])
            {
                possibleIndexes[currentRow].Add(currentCulumn);
                currentCulumn++;
            }
            possibleIndexes[currentRow].Add(currentCulumn);
            if (possibleIndexes[currentRow].Count == 10) possibleIndexes[currentRow].Clear();

            board.CurrentPlayerActions.ImidiateActions[0] = possibleIndexes;
        }

        private int GetCurrentRow(GameBoard board)
        {
            int currentRow = 0;
            bool found = false;
            foreach(var row in board.CurrentlyPlayingLeader.Board)
            {
                foreach(var card in row)
                {
                    if (card == this)
                    {
                        found = true;
                        break;
                    }
                }
                if (found) break;
                currentRow++;
            }
            return currentRow;
        }

        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            DefaultCard playedCard = new Ekimmara();
            board.CurrentlyPlayingLeader.Board[row].Insert(column, playedCard);
            Cooldown(this, 4);
            board.CurrentlyPlayingLeader.UseAbility();
        }

        void IUpdate.StartTurnUpdate()
        {
            if (timeToOrder > 0)
            {
                timeToOrder--;
            }
        }

        public void RespondToBleeding()
        {
            if(timeToOrder > 0)
            {
                timeToOrder--;
            }
        }
    }
}

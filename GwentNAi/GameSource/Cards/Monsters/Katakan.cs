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

        public void Order(GameBoard board)
        {
            PlayCardExpand(board);
        }
        public void Cooldown(int cooldown)
        {
            timeToOrder += (cooldown + 1);
        }

        public void PlayCardExpand(GameBoard board)
        {
            List<List<DefaultCard>> CPboard = board.GetCurrentBoard();
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
            int isInRow = board.GetCurrentBoard()[0].IndexOf(this);
            return (isInRow == -1) ? 1 : 0;
        }

        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            DefaultCard playedCard = new Ekimmara();
            board.GetCurrentBoard()[row].Insert(column, playedCard);
            Cooldown(4);
            board.GetCurrentLeader().UseAbility();
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

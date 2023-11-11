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
    public class Ghoul : DefaultCard, IDeploy, IDeployExpandPickCard
    {
        public Ghoul()
        {
            currentValue = 1;
            maxValue = 1;
            shield = 0;
            provisionValue = 5;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "Ghoul";
            shortName = "Ghoul";
            descriptors = new List<string>() { "Necrophage" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            if (!isMelee(board)) return;

            List<DefaultCard> graveYard = board.CurrentlyPlayingLeader.graveyardDeck.Cards;
            List<int> graveYardIndexes = new List<int>();
            int cardIndex = 0;

            foreach (var card in graveYard)
            {
                if (card.type == "unit" && card.border == 0)
                {
                    graveYardIndexes.Add(cardIndex);
                }
                cardIndex++;
            }

            board.CurrentPlayerActions.ImidiateActions[0][0] = graveYardIndexes;
        }

        public void postPickCardAbility(GameBoard board, int cardIndex)
        {
            DefaultCard consumedCard = board.CurrentlyPlayingLeader.graveyardDeck.Cards[cardIndex];
            currentValue += consumedCard.maxValue;
            maxValue = maxValue - currentValue + consumedCard.maxValue;
            board.CurrentlyPlayingLeader.graveyardDeck.Cards.RemoveAt(cardIndex);
        }

        private bool isMelee(GameBoard board)
        {
            int meleeRow = (board.CurrentlyPlayingLeader == board.Leader1 ? 1 : 0);
            int currentRow = board.CurrentPlayerBoard[0].IndexOf((DefaultCard)this) == -1 ? 1 : 0;
            return currentRow == meleeRow;
        }
    }
}

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

            List<DefaultCard> graveYard = board.GetCurrentLeader().GraveyardDeck.Cards;
            List<int> graveYardIndexes = new List<int>();

            for(int cardIndex = 0; cardIndex < graveYard.Count; cardIndex++)
            {
                DefaultCard card = graveYard[cardIndex];
                if (card.type == "unit" && card.border == 0)
                {
                    graveYardIndexes.Add(cardIndex);
                }
            }

            board.CurrentPlayerActions.ImidiateActions[0][0] = graveYardIndexes;
        }

        public void postPickCardAbility(GameBoard board, int cardIndex)
        {
            DefaultCard consumedCard = board.GetCurrentLeader().GraveyardDeck.Cards[cardIndex];
            currentValue += consumedCard.maxValue;
            maxValue = maxValue - currentValue + consumedCard.maxValue;
            board.GetCurrentLeader().GraveyardDeck.Cards.RemoveAt(cardIndex);
        }

        private bool isMelee(GameBoard board)
        {
            int meleeRow = (board.GetCurrentLeader() == board.Leader1 ? 1 : 0);
            int currentRow = board.GetCurrentBoard()[0].IndexOf((DefaultCard)this) == -1 ? 1 : 0;
            return currentRow == meleeRow;
        }
    }
}

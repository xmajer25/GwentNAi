using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Ozzrel : DefaultCard, IDeploy, IDeployExpandPickCard
    {
        private bool isMelee;
        private List<DefaultCard> graveYard = new List<DefaultCard>(25);
        public Ozzrel()
        {
            currentValue = 1;
            maxValue = 1;
            shield = 0;
            provisionValue = 8;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Ozzrel";
            shortName = "Ozzrel";
            descriptors = new List<string>() { "Necrophage" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            int meleeRow = (board.GetCurrentLeader() == board.Leader1 ? 1 : 0);
            int currentRow = GetCurrentRow(board);
            isMelee = (meleeRow == currentRow);
            List<int> graveYardIndexes = new List<int>();
            int cardIndex = 0;

            if (isMelee)
            {
                graveYard = (board.GetCurrentLeader() == board.Leader1
                    ? board.Leader2.GraveyardDeck.Cards
                    : board.Leader1.GraveyardDeck.Cards);
            }
            else
            {
                graveYard = board.GetCurrentLeader().GraveyardDeck.Cards;
            }

            foreach(var card in graveYard)
            {
                if(card.type == "unit")
                {
                    graveYardIndexes.Add(cardIndex);
                }
                cardIndex++;
            }


            if(graveYardIndexes.Count > 0)
            {
                board.CurrentPlayerActions.ImidiateActions[0][0] = graveYardIndexes;
            }
            else
            {
                board.CurrentPlayerActions.ImidiateActions[0][0].Clear();
            }
        }

        public void postPickCardAbility(GameBoard board, int cardIndex)
        {
            try
            {
                DefaultCard consumedCard = graveYard[cardIndex];
                currentValue += consumedCard.maxValue;
                maxValue = maxValue - currentValue + consumedCard.maxValue;
                graveYard.RemoveAt(cardIndex);
            }catch(Exception ex) { }
            
        }

        private int GetCurrentRow(GameBoard board)
        {
            int currentRow = 0;
            bool found = false;
            foreach (var row in board.GetCurrentBoard())
            {
                foreach (var card in row)
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
    }
}

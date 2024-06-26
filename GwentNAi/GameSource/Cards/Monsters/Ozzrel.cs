﻿using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Ozzrel : DefaultCard, IDeploy, IDeployExpandPickCard
    {
        private bool isMelee;
        private List<DefaultCard> graveYard = new List<DefaultCard>(25);

        /*
         * Initialize information about specific card 
         */
        public Ozzrel()
        {
            CurrentValue = 1;
            MaxValue = 1;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Ozzrel";
            ShortName = "Ozzrel";
            Descriptors = new List<string>() { "Necrophage" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * Deploy ability
         * Depending on row number this unit is in,
         * select a graveyard (ally, opponent)
         * fill imidiate actions with possible targets from this graveyard
         */
        public void Deploy(GameBoard board)
        {
            int meleeRow = (board.GetCurrentLeader() == board.Leader1 ? 1 : 0);
            int currentRow = GetCurrentRow(board);
            isMelee = (meleeRow == currentRow);
            List<int> graveYardIndexes = new();

            if (isMelee)
            {
                graveYard = (board.GetCurrentLeader() == board.Leader1
                    ? board.Leader2.Graveyard.Cards
                    : board.Leader1.Graveyard.Cards);
            }
            else
            {
                graveYard = board.GetCurrentLeader().Graveyard.Cards;
            }

            for (int cardIndex = 0; cardIndex < graveYard.Count; cardIndex++)
            {
                DefaultCard card = graveYard[cardIndex];
                if (card.Type == "unit")
                {
                    graveYardIndexes.Add(cardIndex);
                }
            }



            if (graveYardIndexes.Count > 0)
            {
                board.CurrentPlayerActions.ImidiateActions[0][0] = graveYardIndexes;
            }
            else
            {
                board.CurrentPlayerActions.ImidiateActions[0][0].Clear();
            }
        }

        /*
         * Consumes a card from graveyard 
         */
        public void postPickCardAbility(GameBoard board, int cardIndex)
        {
            try
            {
                DefaultCard consumedCard = graveYard[cardIndex];
                CurrentValue += consumedCard.MaxValue;
                MaxValue = MaxValue - CurrentValue + consumedCard.MaxValue;
                graveYard.RemoveAt(cardIndex);
            }
            catch (Exception ex) { }

        }

        /*
         * Returns row number of this card
         */
        private int GetCurrentRow(GameBoard board)
        {
            int isInRow = board.GetCurrentBoard()[0].IndexOf(this);
            return (isInRow == -1) ? 1 : 0;
        }
    }
}

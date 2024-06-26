﻿using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Griffin : DefaultCard, IDeploy, IDeployExpandPickAlly
    {
        /*
         * Initialize information about specific card 
         */
        public Griffin()
        {
            CurrentValue = 9;
            MaxValue = 9;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Griffin";
            ShortName = "Griffin";
            Descriptors = new List<string>() { "Beast" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * On deploy:
         * Fill imidiate actions with possible targets ->
         * all allied card on the same row as this
         */
        public void Deploy(GameBoard board)
        {
            board.CurrentPlayerActions.ClearImidiateActions();
            List<List<DefaultCard>> AllyBoard = board.GetCurrentBoard();
            int row = GetCurrentRow(AllyBoard);

            //Destroy card if only griffin is in row
            if (AllyBoard[row].Count == 1)
            {
                board.CurrentPlayerActions.ClearImidiateActions();
                this.TakeDemage(CurrentValue, true, board);
                return;
            }

            //Fill possible card target options
            for (int card = 0; card < AllyBoard[row].Count; card++)
            {
                board.CurrentPlayerActions.ImidiateActions[0][row].Add(card);
            }

            //Remove this griffin from possible targets
            board.CurrentPlayerActions.ImidiateActions[0][row].Remove(AllyBoard[row].IndexOf(this));
        }

        /*
         * Returns row number of this card
         */
        public int GetCurrentRow(List<List<DefaultCard>> AllyBoard)
        {
            int isInRow = AllyBoard[0].IndexOf(this);
            return (isInRow == -1) ? 1 : 0;
        }

        /*
         * Executes deploy:
         * Destroys allied card on row-index
         */
        public void PostPickAllyAbilitiy(GameBoard board, int row, int index)
        {
            DefaultCard DestroyedAlly = board.GetCurrentBoard()[row][index];
            DestroyedAlly.TakeDemage(DestroyedAlly.CurrentValue, true, board);
        }
    }
}

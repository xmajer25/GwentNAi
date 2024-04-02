﻿using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class WildHuntHound : DefaultCard, IEndTurnUpdate
    {
        public WildHuntHound()
        {
            CurrentValue = 3;
            MaxValue = 3;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "WildHuntHound";
            ShortName = "WH.Hound";
            Descriptors = new List<string>() { "Beast", "Wild Hunt" };
            TimeToOrder = -1;
            Bleeding = 0;
        }

        public void EndTurnUpdate(GameBoard board)
        {
            if (!IsDominant(board)) return;

            CurrentValue++;
            if (CurrentValue >= MaxValue) MaxValue++;
        }

        private bool IsDominant(GameBoard board)
        {
            List<List<DefaultCard>> enemiePlayerBoard = board.GetEnemieBoard();

            DefaultCard currentMax = board.GetCurrentBoard()
                .SelectMany(list => list)
                .OrderByDescending(obj => obj.CurrentValue)
                .FirstOrDefault();

            DefaultCard enemieMax = enemiePlayerBoard
                .SelectMany(list => list)
                .OrderByDescending(obj => obj.CurrentValue)
                .FirstOrDefault();

            if (enemieMax == null) return true;
            if (currentMax != null)
            {
                return currentMax.CurrentValue >= enemieMax.CurrentValue;
            }
            return true;
        }
    }
}

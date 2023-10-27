﻿using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Player.Monsters
{
    public class ForceOfNature : DefaultLeader, IPlayCardExpand
    {
        public ForceOfNature()
        {
            provisionValue = 15;
            leaderName = "ForceOfNature";
            leaderFaction = "monsters";
            abilityCharges = 1;
            hasPassed = false;
        }

        public override void Order(GameBoard board)
        {
            if (abilityCharges == 0) return;
            PlayCardExpand(board);
        }

        public void PlayCardExpand(GameBoard board)
        {
            List<List<DefaultCard>> CPboard = board.CurrentlyPlayingLeader.Board;
            List<List<int>> possibleIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            int currentRow = 0;
            int currentCulumn = 0;

            foreach (var row in CPboard)
            {
                foreach (var card in row)
                {
                    possibleIndexes[currentRow].Add(currentCulumn);
                    currentCulumn++;
                }
                possibleIndexes[currentRow].Add(currentCulumn);
                if (possibleIndexes[currentRow].Count == 10) possibleIndexes[currentRow].Clear();
                currentRow++;
                currentCulumn = 0;
            }

            board.CurrentPlayerActions.ImidiateActions[0] = possibleIndexes;
        }

        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            DefaultCard playedCard = new WoodlandSpirit();
            board.CurrentlyPlayingLeader.Board[row].Insert(column, playedCard);
            abilityCharges--;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
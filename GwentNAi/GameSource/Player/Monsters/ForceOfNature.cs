using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwentNAi.GameSource.Cards.IExpand;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Player.Monsters
{
    public class ForceOfNature : DefaultLeader, IPlayCardExpand
    {
        public ForceOfNature()
        {
            ProvisionValue = 15;
            LeaderName = "ForceOfNature";
            LeaderFaction = "monsters";
            AbilityCharges = 1;
            HasPassed = false;
        }

        public override object Clone()
        {
            DefaultLeader clonedLeader = new ForceOfNature()
            {
                ProvisionValue = ProvisionValue,
                LeaderName = LeaderName,
                LeaderFaction = LeaderFaction,
                IsStarting = IsStarting,
                Victories = Victories,
                AbilityCharges = AbilityCharges,
                PlayerMethod = PlayerMethod,
                HasPassed = HasPassed,
                HasPlayedCard = HasPlayedCard,
                HasUsedAbility = HasUsedAbility,
                StartingDeck = (Deck)StartingDeck.Copy(),
                HandDeck = (Deck)HandDeck.Copy(),
                GraveyardDeck = (Deck)GraveyardDeck.Copy(),
                Board = Board.Select(innerList => innerList.Select(card => (DefaultCard)card.Clone()).ToList()).ToList(),
            };

            return clonedLeader;
        }

        public override void Order(GameBoard board)
        {
            if (AbilityCharges == 0) return;
            PlayCardExpand(board);
        }

        public void PlayCardExpand(GameBoard board)
        {
            List<List<int>> possibleIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            int currentRow = 0;
            int currentCulumn = 0;

            foreach (var row in Board)
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
            PlayCard(playedCard, row, column, board);
            AbilityCharges--;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}

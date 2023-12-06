using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IExpand;
using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Player.Monsters
{
    public class ArachasSwarm : DefaultLeader, IPlayCardExpand
    {
        public ArachasSwarm() 
        {
            ProvisionValue = 15;
            LeaderName = "ArachasSwarm";
            LeaderFaction = "monsters";
            AbilityCharges = 5;
            HasPassed = false;
        }

        public override void Order(GameBoard board)
        {
            if (AbilityCharges == 0) return;
            PlayCardExpand(board);
        }

        public override object Clone()
        {
            DefaultLeader clonedLeader = new ArachasSwarm()
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

        public void PlayCardExpand(GameBoard board)
        {
            List<List<DefaultCard>> CPboard = board.GetCurrentBoard();
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
            DefaultCard playedCard = new Drone();
            board.GetCurrentBoard()[row].Insert(column, playedCard);
            AbilityCharges--;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}

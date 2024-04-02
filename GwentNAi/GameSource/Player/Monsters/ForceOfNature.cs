using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IExpand;
using GwentNAi.GameSource.Cards.Monsters;
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
                Iterations = Iterations,
                Simulations = Simulations,
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
                StartingDeck = (DefaultDeck)StartingDeck.Copy(),
                Hand = (DefaultDeck)Hand.Copy(),
                Graveyard = (DefaultDeck)Graveyard.Copy(),
                Board = Board
                .Select(innerList => innerList
                    .Select(card => (DefaultCard)card.Clone())  // Deep clone of each DefaultCard
                    .ToList())
                .ToList()
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
            int currentRow = 0;
            int currentCulumn = 0;

            foreach (var row in Board)
            {
                foreach (var card in row)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentCulumn);
                    currentCulumn++;
                }
                board.CurrentPlayerActions.ImidiateActions[0][currentRow].Add(currentCulumn);
                if (board.CurrentPlayerActions.ImidiateActions[0][currentRow].Count == 10) board.CurrentPlayerActions.ImidiateActions[0][currentRow].Clear();
                currentRow++;
                currentCulumn = 0;
            }
        }

        public void PostPlayCardOrder(GameBoard board, int row, int column)
        {
            DefaultCard playedCard = new WoodlandSpirit();
            PlayCardByAbility(playedCard, row, column, board);
            PostAbilitySettings();
        }
    }
}

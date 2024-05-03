using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    /*
     * Child class of a DefaultCard implementign a specific card
     */
    public class Ghoul : DefaultCard, IDeploy, IDeployExpandPickCard
    {
        /*
         * Initialize information about specific card 
         */
        public Ghoul()
        {
            CurrentValue = 1;
            MaxValue = 1;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Ghoul";
            ShortName = "Ghoul";
            Descriptors = new List<string>() { "Necrophage" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        /*
         * On deploy (if melee row):
         * fills imidiate actions with possible targets
         * (Graveyard units without border)
         */
        public void Deploy(GameBoard board)
        {
            if (!isMelee(board)) return;

            List<DefaultCard> graveYard = board.GetCurrentLeader().Graveyard.Cards;
            List<int> graveYardIndexes = new List<int>();

            for (int cardIndex = 0; cardIndex < graveYard.Count; cardIndex++)
            {
                DefaultCard card = graveYard[cardIndex];
                if (card.Type == "unit" && card.Border == 0)
                {
                    graveYardIndexes.Add(cardIndex);
                }
            }

            board.CurrentPlayerActions.ImidiateActions[0][0] = graveYardIndexes;
        }

        /*
         * Executes deploy
         * Consumes targeted card from graveyard
         */
        public void postPickCardAbility(GameBoard board, int cardIndex)
        {
            DefaultCard consumedCard = board.GetCurrentLeader().Graveyard.Cards[cardIndex];
            CurrentValue += consumedCard.MaxValue;
            MaxValue = MaxValue - CurrentValue + consumedCard.MaxValue;
            board.GetCurrentLeader().Graveyard.Cards.RemoveAt(cardIndex);
        }

        /*
         * Returns true if card is placed in the front row
         */
        private bool isMelee(GameBoard board)
        {
            int meleeRow = (board.GetCurrentLeader() == board.Leader1 ? 1 : 0);
            int currentRow = board.GetCurrentBoard()[0].IndexOf((DefaultCard)this) == -1 ? 1 : 0;
            return currentRow == meleeRow;
        }
    }
}
